namespace QANT.PRTG
{
    /// <summary>
    /// Arguments for Remote Host Connectivity
    /// </summary>
    public class CredentialsNetconf
    {
        /// <summary>
        /// The IP address/DNS name entry of the target device
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The SSH Port to use for NETCONF
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The user name for NETCONF access
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// The password for NETCONF access
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// Credentials
        /// </summary>
        public CredentialsNetconf()
        {
            Host = "localhost";
            Port = 22;
            User = "";
            Password = "";
        }
    }
}

