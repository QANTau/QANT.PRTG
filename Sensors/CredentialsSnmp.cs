namespace QANT.PRTG
{
    /// <summary>
    /// Arguments for Remote Host Connectivity
    /// </summary>
    public class CredentialsSnmp
    {
        /// <summary>
        /// The IP address/DNS name entry of the target device
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// v1/v2c Community for SNMP Queries
        /// </summary>
        public string Community { get; set; }

        /// <summary>
        /// Credentials
        /// </summary>
        public CredentialsSnmp()
        {
            Host = "localhost";
            Community = "public";
        }
    }
}