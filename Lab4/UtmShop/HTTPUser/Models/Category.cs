using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPUser.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ItemsCount { get; set; }
    }
}
