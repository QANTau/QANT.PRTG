// Originally Imported from https://github.com/mbettsteller/PrtgSensors

namespace QANT.PRTG
{
    /// <summary>
    /// Unit
    /// </summary>
    public struct Unit
    {
        /// <summary>
        /// BytesBandwidth
        /// </summary>
        public const string BytesBandwidth = "BytesBandwidth";

        /// <summary>
        /// BytesMemory
        /// </summary>
        public const string BytesMemory = "BytesMemory";

        /// <summary>
        /// BytesDisk
        /// </summary>
        public const string BytesDisk = "BytesDisk";

        /// <summary>
        /// Temperature
        /// </summary>
        public const string Temperature = "Temperature";

        /// <summary>
        /// Percent
        /// </summary>
        public const string Percent = "Percent";

        /// <summary>
        /// TimeResponse
        /// </summary>
        public const string TimeResponse = "TimeResponse";

        /// <summary>
        /// TimeSeconds
        /// </summary>
        public const string TimeSeconds = "TimeSeconds";

        /// <summary>
        /// Custom
        /// </summary>
        public const string Custom = "Custom";

        /// <summary>
        /// Count
        /// </summary>
        public const string Count = "Count";

        /// <summary>
        /// Cpu
        /// </summary>
        public const string Cpu = "Cpu (*)";

        /// <summary>
        /// BytesFile
        /// </summary>
        public const string BytesFile = "BytesFile";

        /// <summary>
        /// SpeedDisk
        /// </summary>
        public const string SpeedDisk = "SpeedDisk";

        /// <summary>
        /// SpeedNet
        /// </summary>
        public const string SpeedNet = "SpeedNet";

        /// <summary>
        /// TimeHours
        /// </summary>
        public const string TimeHours = "TimeHours";

        /// <summary>
        /// Returns the Hash Code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return BytesBandwidth.GetHashCode() ^ BytesMemory.GetHashCode() ^ BytesDisk.GetHashCode() ^
                   Temperature.GetHashCode() ^ Percent.GetHashCode() ^ TimeResponse.GetHashCode() ^
                   TimeSeconds.GetHashCode() ^ Custom.GetHashCode() ^
                   Count.GetHashCode() ^ Cpu.GetHashCode() ^ BytesFile.GetHashCode() ^
                   SpeedDisk.GetHashCode() ^ SpeedNet.GetHashCode() ^ TimeHours.GetHashCode();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Unit))
                return false;

            return Equals((Unit)obj);
        }

        /// <summary>
        /// Equal To
        /// </summary>
        /// <param name="unit1"></param>
        /// <param name="unit2"></param>
        /// <returns></returns>
        public static bool operator == (Unit unit1, Unit unit2)
        {
            return (unit1.Equals(unit2));
        }

        /// <summary>
        /// Not Equal To
        /// </summary>
        /// <param name="unit1"></param>
        /// <param name="unit2"></param>
        /// <returns></returns>
        public static bool operator != (Unit unit1, Unit unit2)
        {
            return !(unit1.Equals(unit2));
        }
    }
}