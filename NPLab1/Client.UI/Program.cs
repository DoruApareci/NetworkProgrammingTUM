using System.Runtime.CompilerServices;

public class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Client";
        ClientApp app = new ClientApp();
        app.Main();
    }
}