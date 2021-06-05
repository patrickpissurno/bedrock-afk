using BedrockAFK.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BedrockAFK.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
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
                    Main(cancel).Wait();
                }
                finally
                {
                    Console.CancelKeyPress -= dispose;
                }
            }
        }

        static async Task Main(CancellationTokenSource cancel)
        {
            MainCore.Mode? mode = null;
            while (mode == null)
            {
                Console.WriteLine("Bedrock AFK");
                Console.WriteLine("-----------");
                Console.WriteLine("Choose a mode to start:");
                Console.WriteLine("1) Walk forward");
                Console.WriteLine("2) Water bucket");
                Console.WriteLine("3) AFK Fishing (fast)");
                Console.WriteLine("4) AFK Fishing (slow)");
                if (int.TryParse(Console.ReadLine(), out var num) && num >= 1 && num <= 4)
                    mode = (MainCore.Mode)num;
                else
                    Console.WriteLine("Invalid option.\n");
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

            try
            {
                var core = new MainCore();
                await core.Run(cancel, mode.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error. Stack:");
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
