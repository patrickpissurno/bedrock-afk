using BedrockAFK.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BedrockAFK.CLI
{
    class Program
    {
        private static MainCore.Mode? ParseMode(string x) => int.TryParse(x, out var n) && n >= 1 && n <= 4 ? (MainCore.Mode)n : (MainCore.Mode?)null;

        static void Main(string[] args)
        {
            var _mode = args.Length < 1 ? null : args[0];
            var mode = ParseMode(_mode);
            if (_mode != null && mode == null)
            {
                Environment.ExitCode = 1;
                Console.Error.WriteLine("Invalid mode");
                return;
            }

            using (var cancel = new CancellationTokenSource())
            {
                ConsoleCancelEventHandler dispose = (o, e) =>
                {
                    e.Cancel = true;
                    if (!cancel.IsCancellationRequested)
                    {
                        cancel.Cancel();
                        Console.WriteLine("Stopping...");
                    }
                };
                Console.CancelKeyPress += dispose;
                try
                {
                    Main(cancel, mode).Wait();
                }
                catch (Exception ex)
                {
                    Environment.ExitCode = 1;
                    Console.Error.WriteLine("Fatal error. Stack:");
                    Console.Error.WriteLine(ex.ToString());

                    if (mode == null)
                    {
                        Console.WriteLine("\nPress any key to exit.");
                        Console.ReadKey();
                    }
                }
                finally
                {
                    Console.CancelKeyPress -= dispose;
                }
            }
        }

        static async Task Main(CancellationTokenSource cancel, MainCore.Mode? mode)
        {
            while (mode == null && !cancel.IsCancellationRequested)
            {
                Console.WriteLine("Bedrock AFK");
                Console.WriteLine("-----------");
                Console.WriteLine("Choose a mode to start:");
                Console.WriteLine("1) Walk forward");
                Console.WriteLine("2) Water bucket");
                Console.WriteLine("3) AFK Fishing (fast)");
                Console.WriteLine("4) AFK Fishing (slow)");

                var _mode = ParseMode(Console.ReadLine());
                if (_mode == null)
                {
                    await Task.Delay(100);
                    if (!cancel.IsCancellationRequested)
                        Console.WriteLine("Invalid option.\n");
                    else
                        return;
                }
                else
                    mode = _mode;
            }

            for (var i = 5; i >= 1; i--)
            {
                Console.WriteLine("Starting... " + i);
                if(!cancel.IsCancellationRequested)
                    await Task.Delay(1000);
            }

            if (cancel.IsCancellationRequested)
                return;

            Console.WriteLine("Started.");

            var core = new MainCore();
            await core.Run(cancel, mode.Value);
        }
    }
}
