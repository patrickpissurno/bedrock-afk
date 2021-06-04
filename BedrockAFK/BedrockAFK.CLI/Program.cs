using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Input.Preview.Injection;

namespace BedrockAFK.CLI
{
    class Program
    {
        private const uint WM_GETTEXT = 0x000D;
        private const int SW_SHOW = 5;
        private const int SW_SHOWDEFAULT = 10;

        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool ShowWindowAsync(IntPtr windowHandle, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetForegroundWindow(IntPtr windowHandle);

        static IEnumerable<IntPtr> EnumerateProcessWindowHandles(Process process)
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in process.Threads)
                EnumThreadWindows(thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }

        static void Main(string[] args)
        {
            Main().Wait();
        }

        static async Task SimulateKeyPress(InputInjector injector, IntPtr window, VirtualKey key, int duration)
        {
            ShowWindowAsync(window, SW_SHOWDEFAULT);
            ShowWindowAsync(window, SW_SHOW);
            SetForegroundWindow(window);

            await Task.Delay(2);

            //info.VirtualKey = (ushort)((VirtualKey)Enum.Parse(typeof(VirtualKey), "A", true));

            injector.InjectKeyboardInput(new[] { new InjectedInputKeyboardInfo { VirtualKey = (ushort)key } });

            await Task.Delay(duration);

            injector.InjectKeyboardInput(new[] { new InjectedInputKeyboardInfo { VirtualKey = (ushort)key, KeyOptions = InjectedInputKeyOptions.KeyUp } });
            //Console.WriteLine("SIMULATED");
        }

        static async Task SimulateMousePress(InputInjector injector, IntPtr window, InjectedInputMouseOptions mouse, int duration)
        {
            ShowWindowAsync(window, SW_SHOWDEFAULT);
            ShowWindowAsync(window, SW_SHOW);
            SetForegroundWindow(window);

            await Task.Delay(2);

            //info.VirtualKey = (ushort)((VirtualKey)Enum.Parse(typeof(VirtualKey), "A", true));

            injector.InjectMouseInput(new[] { new InjectedInputMouseInfo { MouseOptions = mouse } });

            await Task.Delay(duration);

            InjectedInputMouseOptions up = InjectedInputMouseOptions.None;
            switch (mouse)
            {
                case InjectedInputMouseOptions.LeftDown:
                    up = InjectedInputMouseOptions.LeftUp;
                    break;
                case InjectedInputMouseOptions.MiddleDown:
                    up = InjectedInputMouseOptions.MiddleUp;
                    break;
                case InjectedInputMouseOptions.RightDown:
                    up = InjectedInputMouseOptions.RightUp;
                    break;
            }
            if(up != InjectedInputMouseOptions.None)
                injector.InjectMouseInput(new[] { new InjectedInputMouseInfo { MouseOptions = up } });
            //Console.WriteLine("SIMULATED");
        }

        static async Task Main()
        {
            int mode = -1;
            while (mode == -1)
            {
                Console.WriteLine("Bedrock AFK");
                Console.WriteLine("-----------");
                Console.WriteLine("Choose a mode to start:");
                Console.WriteLine("1) Walk forward");
                Console.WriteLine("2) Water bucket");
                if (int.TryParse(Console.ReadLine(), out var num) && num > 0 && num <= 2)
                    mode = num;
                else
                    Console.WriteLine("Invalid option.\n");
            }

            Console.WriteLine("Starting...");

            await Task.Delay(2500);

            Console.WriteLine("Started.");

            try
            {
                var injector = InputInjector.TryCreate();

                while (true)
                {
                    foreach (var process in Process.GetProcessesByName("ApplicationFrameHost"))
                    {
                        foreach (var wHandle in EnumerateProcessWindowHandles(process))
                        {
                            var windowTitle = new StringBuilder(1000);
                            SendMessage(wHandle, WM_GETTEXT, windowTitle.Capacity, windowTitle);

                            //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, windowTitle);

                            if (windowTitle.ToString() == "Minecraft")
                            {
                                switch (mode)
                                {
                                    case 1:
                                        _ = SimulateKeyPress(injector, wHandle, VirtualKey.W, 250);
                                        break;
                                    case 2:
                                        _ = SimulateMousePress(injector, wHandle, InjectedInputMouseOptions.RightDown, 100);
                                        break;
                                }
                            }
                        }
                    }
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error. Stack:");
                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();
        }
    }
}
