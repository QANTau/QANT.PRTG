// Originally Imported from https://github.com/mbettsteller/PrtgSensors

namespace QANT.PRTG
{
    /// <summary>
    /// Speed or Volume Size
    /// </summary>
    public struct SpeedOrVolumeSize
    {
        /// <summary>
        /// One
        /// </summary>
        public const string One = "One";

        /// <summary>
        /// Kilo
        /// </summary>
        public const string Kilo = "Kilo";

        /// <summary>
        /// Mega
        /// </summary>
        public const string Mega = "Mega";

        /// <summary>
        /// Giga
        /// </summary>
        public const string Giga = "Giga";

        /// <summary>
        /// Tera
        /// </summary>
        public const string Tera = "Tera";

        /// <summary>
        /// Byte
        /// </summary>
        public const string Byte = "Byte";

        /// <summary>
        /// Kilobyte
        /// </summary>
        public const string Kilobyte = "Kilobyte";

        /// <summary>
        /// Megabyte
        /// </summary>
        public const string Megabyte = "Megabyte";

        /// <summary>
        /// Gigabyte
        /// </summary>
        public const string Gigabyte = "Gigabyte";

        /// <summary>
        /// Terabyte
        /// </summary>
        public const string Terabyte = "Terabyte";

        /// <summary>
        /// Bit
        /// </summary>
        public const string Bit = "Bit";

        /// <summary>
        /// Kilobit
        /// </summary>
        public const string Kilobit = "Kilobit";

        /// <summary>
        /// Megabit
        /// </summary>
        public const string Megabit = "Megabit";

        /// <summary>
        /// Gigabit
        /// </summary>
        public const string Gigabit = "Gigabit";

        /// <summary>
        /// Terabit
        /// </summary>
        public const string Terabit = "Terabit";

        /// <summary>
        /// Returns the Hash Code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return One.GetHashCode() ^ Kilo.GetHashCode() ^ Mega.GetHashCode() ^
                   Giga.GetHashCode() ^ Tera.GetHashCode() ^ Byte.GetHashCode() ^
                   Kilobyte.GetHashCode() ^ Megabyte.GetHashCode() ^ Gigabyte.GetHashCode() ^
                   Terabyte.GetHashCode() ^ Bit.GetHashCode() ^ Kilobit.GetHashCode() ^ 
                   Megabit.GetHashCode() ^ Gigabit.GetHashCode() ^ Terabit.GetHashCode();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SpeedOrVolumeSize))
                return false;

            return Equals((SpeedOrVolumeSize)obj);
        }

        /// <summary>
        /// Equal To
        /// </summary>
        /// <param name="size1"></param>
        /// <param name="size2"></param>
        /// <returns></returns>
        public static bool operator == (SpeedOrVolumeSize size1, SpeedOrVolumeSize size2)
        {
            return (size1.Equals(size2));
        }

        /// <summary>
        /// Not Equal To
        /// </summary>
        /// <param name="size1"></param>
        /// <param name="size2"></param>
        /// <returns></returns>
        public static bool operator != (SpeedOrVolumeSize size1, SpeedOrVolumeSize size2)
        {
            return !(size1.Equals(size2));
        }
    }
}