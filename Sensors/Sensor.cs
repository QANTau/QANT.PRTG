using CLAP;
using System;

namespace QANT.PRTG
{
    internal class Sensor
    {
        [Empty, Help]
        public static void Help(string help)
        {
            Console.WriteLine(help);
        }

        [Verb(Description = "Credentials for Netconf Sensors")]
        public static void Netconf(
            [Aliases("t"), Description("Target Host Name/IP Address"), Required]
            string target,
            [Aliases("p"), Description("SSH Port")]
            int port,
            [Aliases("u"), Description("SSH User Name"), Required]
            string user,
            [Aliases("s"), Description("SSH Password"), Required]
            string password,
            [Aliases("q"), Description("Query"), Required]
            string query,
            [Aliases("d"), Description("Debug")] bool debug = false
        )
        {

            // Setup Credentials
            var credentials = new CredentialsNetconf
            {
                Host = target,
                Port = port,
                User = user,
                Password = password
            };

            // Host Name Check
            if (!Dns.Check(target))
            {
                System.Windows.Forms.Application.Exit();
            }


        }

        [Verb(Description = "SNMP (v1/v2c Credentials for SNMP Sensors", IsDefault = true)]
        public static void Snmp(
            [Aliases("t"), Description("Target Host Name/IP Address"), Required]
                    string target,
            [Aliases("c"), Description("SNMP (v1/v2c) Community"), Required]
                    string community,
            [Aliases("q"), Description("Query"), Required]
                    string query,
            [Aliases("d"), Description("Debug")]
                    bool debug = false
            )
        {

            // Setup Credentials
            var credentials = new CredentialsSnmp
            {
                Host = target,
                Community = community
            };

            // Host Name Check
            if (!Dns.Check(target))
            {
                System.Windows.Forms.Application.Exit();
            }

            query = query.ToLower();
            switch (query)
            {
                case "cps-ups":
                    CPS.Ups.Check(credentials);
                    break;
                case "eaton-ups":
                    Eaton.Ups.Check(credentials);
                    break;
                case "socomec-ups":
                    Socomec.Ups.Check(credentials);
                    break;
                default:
                    Error.WriteOutput("Query '" + query + "' is Unknown");
                    break;
            }


        }
    }
}
