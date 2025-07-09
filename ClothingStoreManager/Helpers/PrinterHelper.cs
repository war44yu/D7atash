using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Drawing;

namespace ClothingStoreManager.Helpers
{
    public static class PrinterHelper
    {
        // طباعة نص عادي
        public static void PrintText(string text, string printerName = "")
        {
            PrintDocument pd = new PrintDocument();
            if (!string.IsNullOrEmpty(printerName))
                pd.PrinterSettings.PrinterName = printerName;
            pd.PrintPage += (s, e) =>
            {
                if (e.Graphics != null)
                    e.Graphics.DrawString(text, new Font("Arial", 12), Brushes.Black, 10, 10);
            };
            pd.Print();
        }

        // فتح درج الكاشير عبر كود ESC/POS
        public static void OpenCashDrawer(string printerName = "")
        {
            if (string.IsNullOrEmpty(printerName))
                printerName = new PrinterSettings().PrinterName;
            RawPrinterHelper.SendStringToPrinter(printerName, "\x1B\x70\x00\x19\xFA");
        }
    }

    // كلاس مساعد لإرسال أوامر مباشرة للطابعة
    public class RawPrinterHelper
    {
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter(string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int level, IntPtr di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

        public static bool SendStringToPrinter(string printerName, string data)
        {
            IntPtr pPrinter;
            if (!OpenPrinter(printerName.Normalize(), out pPrinter, IntPtr.Zero)) return false;
            var bytes = System.Text.Encoding.Default.GetBytes(data);
            int written;
            StartDocPrinter(pPrinter, 1, IntPtr.Zero);
            StartPagePrinter(pPrinter);
            var pUnmanagedBytes = Marshal.AllocCoTaskMem(bytes.Length);
            Marshal.Copy(bytes, 0, pUnmanagedBytes, bytes.Length);
            WritePrinter(pPrinter, pUnmanagedBytes, bytes.Length, out written);
            EndPagePrinter(pPrinter);
            EndDocPrinter(pPrinter);
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            ClosePrinter(pPrinter);
            return true;
        }
    }
}