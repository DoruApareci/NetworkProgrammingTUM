using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmailClient.Models
{
    public class ConfigLoader
    {
        public static EmailSettings LoadConfig(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<EmailSettings>(json);
        }
    }
}
