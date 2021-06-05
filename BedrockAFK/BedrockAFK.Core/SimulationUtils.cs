using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Input.Preview.Injection;

namespace BedrockAFK.Core
{
    internal static class SimulationUtils
    {
        public static async Task SimulateKeyPress(InputInjector injector, IntPtr window, VirtualKey key, int duration)
        {
            LowlevelUtils.ShowWindowAsync(window, LowlevelUtils.SW_SHOWDEFAULT);
            LowlevelUtils.ShowWindowAsync(window, LowlevelUtils.SW_SHOW);
            LowlevelUtils.SetForegroundWindow(window);

            await Task.Delay(2);

            //info.VirtualKey = (ushort)((VirtualKey)Enum.Parse(typeof(VirtualKey), "A", true));

            injector.InjectKeyboardInput(new[] { new InjectedInputKeyboardInfo { VirtualKey = (ushort)key } });

            await Task.Delay(duration);

            injector.InjectKeyboardInput(new[] { new InjectedInputKeyboardInfo { VirtualKey = (ushort)key, KeyOptions = InjectedInputKeyOptions.KeyUp } });
        }

        public static async Task SimulateMousePress(InputInjector injector, IntPtr window, InjectedInputMouseOptions mouse, int duration)
        {
            LowlevelUtils.ShowWindowAsync(window, LowlevelUtils.SW_SHOWDEFAULT);
            LowlevelUtils.ShowWindowAsync(window, LowlevelUtils.SW_SHOW);
            LowlevelUtils.SetForegroundWindow(window);

            await Task.Delay(2);

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
            if (up != InjectedInputMouseOptions.None)
                injector.InjectMouseInput(new[] { new InjectedInputMouseInfo { MouseOptions = up } });
        }
    }
}
