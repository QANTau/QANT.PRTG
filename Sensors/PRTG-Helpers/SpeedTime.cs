// Originally Imported from https://github.com/mbettsteller/PrtgSensors

namespace QANT.PRTG
{
    /// <summary>
    /// SpeedTime
    /// </summary>
    public struct SpeedTime
    {
        /// <summary>
        /// Second
        /// </summary>
        public const string Second = "Second";

        /// <summary>
        /// Minute
        /// </summary>
        public const string Minute = "Minute";

        /// <summary>
        /// Hour
        /// </summary>
        public const string Hour = "Hour";

        /// <summary>
        /// Day
        /// </summary>
        public const string Day = "Day";

        /// <summary>
        /// Returns the Hash Code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Second.GetHashCode() ^ Minute.GetHashCode() ^ Hour.GetHashCode() ^ Day.GetHashCode();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SpeedTime))
                return false;

            return Equals((SpeedTime)obj);
        }

        /// <summary>
        /// Equal To
        /// </summary>
        /// <param name="speedTime1"></param>
        /// <param name="speedTime2"></param>
        /// <returns></returns>
        public static bool operator == (SpeedTime speedTime1, SpeedTime speedTime2)
        {
            return (speedTime1.Equals(speedTime2));
        }

        /// <summary>
        /// Not Equal To
        /// </summary>
        /// <param name="speedTime1"></param>
        /// <param name="speedTime2"></param>
        /// <returns></returns>
        public static bool operator != (SpeedTime speedTime1, SpeedTime speedTime2)
        {
            return !(speedTime1.Equals(speedTime2));
        }
    }
}