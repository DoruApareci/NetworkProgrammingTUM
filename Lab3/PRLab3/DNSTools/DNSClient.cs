using System.Net;
using System.Text;

namespace DNSTools
{
    public class DNSClient
    {
        public DNSClient(string dns)
        {
            UseDNS(dns);
        }

        public IPAddress dnsServer { get; private set; }

        public string Resolve(IPAddress ip)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append($"Resolve domains for IP '{ip}'\n");
                var resp = Dns.GetHostEntry(ip).HostName.Split('.');
                sb.Append($"Domain(s) associated with IP '{ip}' - count '{resp.Count()}':\n");
                foreach (var item in resp)
                {
                    sb.Append($"\t{item}\n");
                }
            }
            catch (Exception e)
            {
                sb.Append("Error:/t" + e.Message);
            }
            return sb.ToString();
        }

        public string Resolve(string domain)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append($"Resolve IPs for domain '{domain}'\n");
                IPAddress[] ips = Dns.GetHostAddresses(domain);
                sb.Append($"IPs associated with domain '{domain}' - count '{ips.Count()}':\n");
                foreach (var item in ips)
                {
                    sb.Append($"\t{item}\n");
                }
            }
            catch (Exception e)
            {
                sb.Append("Error:/t" + e.Message);
            }
            return sb.ToString();
        }

        public bool UseDNS(string input)
        {
            try
            {
                if (IPAddress.TryParse(input, out IPAddress ip))
                {
                    dnsServer = Dns.GetHostAddresses(ip.ToString())[0];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}

