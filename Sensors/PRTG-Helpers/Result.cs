// Originally Imported from https://github.com/mbettsteller/PrtgSensors

using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace QANT.PRTG
{
    /// <summary>
    /// Result
    /// </summary>
    public class Result
    {
        private Collection<Channel> _channels = new Collection<Channel>();

        /// <summary>
        /// Channels
        /// </summary>
        public Collection<Channel> Channels
        {
            get { return _channels; }
            //set { channels = value; }
        }
        private string _text;

        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// WriteOutput
        /// </summary>
        public void WriteOutput()
        {
            var prtg =
                new XElement("prtg",
                             new XElement("text", Text)
                    );

            foreach (var channel in Channels)
            {
                var nodelist = new Collection<XElement>();

                if (channel.Description == null)
                {
                    System.Diagnostics.Debugger.Break();
                    return;
                }
                nodelist.Add(new XElement("channel", channel.Description));
                nodelist.Add(new XElement("value", channel.Value));
                nodelist.Add(new XElement("unit", channel.Unit));
                if (channel.CustomUnit != null) nodelist.Add(new XElement("customunit", channel.CustomUnit));
                nodelist.Add(new XElement("speedsize", channel.SpeedSize));
                nodelist.Add(new XElement("volumesize", channel.VolumeSize));
                nodelist.Add(new XElement("speedtime", channel.SpeedTime));
                nodelist.Add(new XElement("mode", channel.Mode));
                nodelist.Add(new XElement("float", channel.Float));
                nodelist.Add(new XElement("decimalmode", channel.DecimalMode));
                nodelist.Add(new XElement("warning", channel.Warning));
                nodelist.Add(new XElement("showchart", channel.ShowChart));
                nodelist.Add(new XElement("showtable", channel.ShowTable));
                if (channel.LimitMaxError != null) nodelist.Add(new XElement("limitmaxerror", channel.LimitMaxError));
                if (channel.LimitMaxWarning != null) nodelist.Add(new XElement("limitmaxwarning", channel.LimitMaxWarning));
                if (channel.LimitMinWarning != null) nodelist.Add(new XElement("limitminwarning", channel.LimitMinWarning));
                if (channel.LimitMinError != null) nodelist.Add(new XElement("limitminerror", channel.LimitMinError));
                if (channel.LimitErrorMessage != null) nodelist.Add(new XElement("limiterrormsg", channel.LimitErrorMessage));
                if (channel.LimitWarningMessage != null) nodelist.Add(new XElement("limitwarningmsg", channel.LimitWarningMessage));
                nodelist.Add(new XElement("limitmode", channel.LimitMode));
                if (channel.ValueLookup != null) nodelist.Add(new XElement("valuelookup", channel.ValueLookup));


                var result = new XElement("result");
                foreach (var node in nodelist)
                {
                    result.AddFirst(node);
                }
                prtg.AddFirst(result);

            }

            Console.WriteLine(prtg);
        }
    }
}