using Server.UI;
public class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Server";
        ServerApp app = new ServerApp();
        app.Main();
    }
}