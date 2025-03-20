using Server.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UI
{
    internal class ServerLogger : ILogger
    {
        private readonly MainWindow _mainWindow;
        public ServerLogger(MainWindow main)
        {
            _mainWindow = main;
        }
        public void Log(string message)
        {
            lock (_mainWindow.Logs)
            {
                _mainWindow.Logs += message + Environment.NewLine;
            }
        }
    }
}
