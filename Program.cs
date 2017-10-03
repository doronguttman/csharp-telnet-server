using System;

namespace TelnetServer
{
    class Program
    {
        public static readonly ConsoleColor DefaultColor = Console.ForegroundColor;

        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            try
            {
                var welcomeScreen = new Screens.Welcome();

                var server = new Comm.Server(23);
                server.NewSession += async (sender, session) =>
                {
                    await session.Send(welcomeScreen);
                    session.Disconnected += (o, e) =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Disconnected");
                        Console.ForegroundColor = DefaultColor;
                    };
                    session.MessageReceived += async (o, message) =>
                    {
                        await session.Send(new Telnet.ClearScreen());
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(message);
                        Console.ForegroundColor = DefaultColor;
                    };
                };
                server.Start();

                Console.ReadLine();

                server.Stop();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
