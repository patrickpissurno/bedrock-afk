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
            //info.VirtualKey = (ushort)((VirtualKey)Enum.Parse(typeof(VirtualKey), "A", true));

            if (LowlevelUtils.GetForegroundWindow() == window)
                injector.InjectKeyboardInput(new[] { new InjectedInputKeyboardInfo { VirtualKey = (ushort)key } });

            await Task.Delay(duration);

            if (LowlevelUtils.GetForegroundWindow() == window)
                injector.InjectKeyboardInput(new[] { new InjectedInputKeyboardInfo { VirtualKey = (ushort)key, KeyOptions = InjectedInputKeyOptions.KeyUp } });
        }

        public static async Task SimulateMousePress(InputInjector injector, IntPtr window, InjectedInputMouseOptions mouse, int duration)
        {
            if (LowlevelUtils.GetForegroundWindow() == window)
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
            {
                if (LowlevelUtils.GetForegroundWindow() == window)
                    injector.InjectMouseInput(new[] { new InjectedInputMouseInfo { MouseOptions = up } });
            }
        }
    }
}
