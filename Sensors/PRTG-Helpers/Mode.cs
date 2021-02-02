// Originally Imported from https://github.com/mbettsteller/PrtgSensors

namespace QANT.PRTG
{
    /// <summary>
    /// Mode
    /// </summary>
    public struct Mode
    {
        /// <summary>
        /// Absolute
        /// </summary>
        public const string Absolute = "Absolute";

        /// <summary>
        /// Difference
        /// </summary>
        public const string Difference = "Difference";

        /// <summary>
        /// Returns the Hash Code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Absolute.GetHashCode() ^ Difference.GetHashCode();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Mode))
                return false;

            return Equals((Mode)obj);
        }

        /// <summary>
        /// Equal To
        /// </summary>
        /// <param name="mode1"></param>
        /// <param name="mode2"></param>
        /// <returns></returns>
        public static bool operator == (Mode mode1, Mode mode2)
        {
            return (mode1.Equals(mode2));
        }

        /// <summary>
        /// Not Equal To
        /// </summary>
        /// <param name="mode1"></param>
        /// <param name="mode2"></param>
        /// <returns></returns>
        public static bool operator != (Mode mode1, Mode mode2)
        {
            return !(mode1.Equals(mode2));
        }
    }
}