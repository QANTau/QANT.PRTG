// Originally Imported from https://github.com/mbettsteller/PrtgSensors

using System;
using System.Globalization;
using System.Xml.Linq;

namespace QANT.PRTG
{
    /// <summary>
    /// Error
    /// </summary>
    public static class Error
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public static void WriteOutput(string text)
        {
            WriteOutput(text,null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="exception"></param>
        public static void WriteOutput(string text, Exception exception)
        {
            if (exception != null)
            {
                text = text + " " + exception.Message + " " + exception.StackTrace;
                System.Diagnostics.Debugger.Break();
            }
            var prtg =
                new XElement("prtg",
                    new XElement("error", 1.ToString(CultureInfo.InvariantCulture)),
                    new XElement("text", text));
            Console.WriteLine(prtg);
        }
    }

}