using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace BedrockAFK.Core
{
    internal class LowlevelUtils
    {
        internal const uint WM_GETTEXT = 0x000D;
        internal const int SW_SHOW = 5;
        internal const int SW_SHOWDEFAULT = 10;

        internal delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        internal static IEnumerable<IntPtr> EnumerateProcessWindowHandles(Process process)
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in process.Threads)
            {
                EnumThreadWindows(thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);
            }

            return handles;
        }
    }
}
