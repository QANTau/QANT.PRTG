// Originally Imported from https://github.com/mbettsteller/PrtgSensors

using System.Globalization;

namespace QANT.PRTG
{
    /// <summary>
    /// Decimal Mode
    /// </summary>
    public struct DecimalMode
    {
        /// <summary>
        /// Auto
        /// </summary>
        public const string Auto = "Auto";

        /// <summary>
        /// All
        /// </summary>
        public const string All = "All";

        /// <summary>
        /// Two
        /// </summary>
        public static readonly string Two = 2.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Returns the Hash Code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Auto.GetHashCode() ^ All.GetHashCode() ^ Two.GetHashCode();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is DecimalMode))
                return false;

            return Equals((DecimalMode)obj);
        }         

        /// <summary>
        /// Equal To
        /// </summary>
        /// <param name="mode1"></param>
        /// <param name="mode2"></param>
        /// <returns></returns>
        public static bool operator == (DecimalMode mode1, DecimalMode mode2)
        {
            return (mode1.Equals(mode2));
        }

        /// <summary>
        /// Not Equal To
        /// </summary>
        /// <param name="mode1"></param>
        /// <param name="mode2"></param>
        /// <returns></returns>
        public static bool operator != (DecimalMode mode1, DecimalMode mode2)
        {
            return !(mode1.Equals(mode2));
        }
    }
}