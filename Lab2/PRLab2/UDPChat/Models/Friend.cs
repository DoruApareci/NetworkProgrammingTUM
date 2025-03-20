using System.Net;

namespace UDPChat.Models;

public class Friend
{
    public string Username { get; set; }
    public IPAddress IP { get; set; }
    public List<string> Messages { get; set; }
}