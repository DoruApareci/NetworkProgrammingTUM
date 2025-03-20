using Server.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UI
{
    internal class ServerMessager : IMessager
    {
        private readonly MainWindow _main;
        public ServerMessager(MainWindow main)
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
}
