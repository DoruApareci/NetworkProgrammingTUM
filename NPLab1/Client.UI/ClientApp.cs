using Client.BL;

public class ClientApp : IClientLogger
{
    bool go = true;
    string username = "";

    public void Main()
    {
        ClientBL clientBl = new ClientBL(this);
        while (go)
        {
            Console.WriteLine("Client UI\n\tInput format:\n\t\t <ServerIP/Any/127.0.0.1>:<ServerPort>:<Username>\n");
            try
            {
                var data = Console.ReadLine().Split(":");
                clientBl = new(this, data[0], int.Parse(data[1]));
                username = data[2];
                go = false;
            }
            catch
            {
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
            }
        }
        clientBl.Connect();
        go = true;
        Console.WriteLine("Client UI\n\tInput \"\\Cancel\" for exiting\n\t<message>[Enter] for sending");
        while (go)
        {
            var command = Console.ReadLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', command.Length)+"\r");
            if (command == "\\Cancel")
            {
                clientBl.Disconnect();
                break;
            }
            clientBl.SendMessage($"@{username}: {command}");
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