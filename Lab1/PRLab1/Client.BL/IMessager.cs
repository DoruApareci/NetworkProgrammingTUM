using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.BL
{
    public interface IMessager
    {
        public void Message(string sender, string message);
    }
}
