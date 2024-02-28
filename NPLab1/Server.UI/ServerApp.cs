using Server.BL;

namespace Server.UI;

public class ServerApp : IServerLogger
{
    bool go = true;

    public void Main()
    {
        ServerBL serverBl = new(this);
        while (go)
        {
            Console.WriteLine("Server UI\n\tInput format:\n\t\t <ServerIP/Any/127.0.0.1>:<ServerPort>\n");
            try
            {
                var data = Console.ReadLine().Split(":");
                serverBl = new(this, data[0], int.Parse(data[1]));
                go = false;
            }
            catch
            {
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
            }
        }
        serverBl.StartServer();
        go = true;
        Console.WriteLine("Server UI\n\tInput \"\\Cancel\" for exiting");
        while (go)
        {
            var command = Console.ReadLine();
            if (command == "\\Cancel")
            {
                serverBl.StopServer();
                go = false;
            }
        }

    }

    public void Log(string message)
    {
        lock (Console.Out)
        {
            Console.WriteLine($"{message}\n");
        }
    }
}
