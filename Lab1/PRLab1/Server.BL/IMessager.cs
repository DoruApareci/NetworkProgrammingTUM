namespace Server.BL
{
    public interface IMessager
    {
        public void Message(string sender, string message);
    }
}