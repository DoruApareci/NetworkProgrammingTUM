using Client.BL;

namespace Client.UI;

class ClientMessager : IMessager
{
    private readonly MainWindow _main;
    public ClientMessager(MainWindow main)
    {
        _main = main;
    }

    public void Message(string sender, string message)
    {
        lock (_main.Messages)
        {
            _main.Messages += ($"{sender}: {message}\n");
        }
    }
}
