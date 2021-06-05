using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Input.Preview.Injection;

namespace BedrockAFK.Core
{
    public class MainCore
    {
        public enum Mode
        {
            WalkForward = 1,
            WaterBucket = 2,
            AFKFishingFast = 3,
            AFKFishingSlow = 4,
        }

        public async Task Run(CancellationTokenSource cancel, Mode mode)
        {
            var injector = InputInjector.TryCreate();

            while (!cancel.IsCancellationRequested)
            {
                int delay = 1000;
                foreach (var process in Process.GetProcessesByName("ApplicationFrameHost"))
                {
                    foreach (var wHandle in LowlevelUtils.EnumerateProcessWindowHandles(process))
                    {
                        var windowTitle = new StringBuilder(1000);
                        LowlevelUtils.SendMessage(wHandle, LowlevelUtils.WM_GETTEXT, windowTitle.Capacity, windowTitle);

                        //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, windowTitle);

                        if (windowTitle.ToString() == "Minecraft")
                        {
                            switch (mode)
                            {
                                case Mode.WalkForward:
                                    _ = SimulationUtils.SimulateKeyPress(injector, wHandle, VirtualKey.W, 250);
                                    break;
                                case Mode.WaterBucket:
                                    _ = SimulationUtils.SimulateMousePress(injector, wHandle, InjectedInputMouseOptions.RightDown, 100);
                                    break;
                                case Mode.AFKFishingFast:
                                    _ = SimulationUtils.SimulateMousePress(injector, wHandle, InjectedInputMouseOptions.RightDown, 50);
                                    delay = 12000;
                                    break;
                                case Mode.AFKFishingSlow:
                                    _ = SimulationUtils.SimulateMousePress(injector, wHandle, InjectedInputMouseOptions.RightDown, 50);
                                    delay = 22000;
                                    break;
                            }
                        }
                    }
                }

                if(!cancel.IsCancellationRequested)
                    await Task.Delay(delay);
            }
        }
    }
}
