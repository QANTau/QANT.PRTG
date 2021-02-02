using System.Net.Sockets;

namespace QANT.PRTG
{
    public class Dns
    {
        public static bool Check(string target)
        {
            try
            {
                System.Net.Dns.GetHostAddresses(target);
            }
            catch (SocketException se)
            {
                Error.WriteOutput("DNS Resolution Error (" + se.Message + ")");
                return false;
            }

            return true;
        }
    }
}
