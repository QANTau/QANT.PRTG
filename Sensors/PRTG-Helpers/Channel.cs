// Originally Imported from https://github.com/mbettsteller/PrtgSensors

using System.Globalization;

namespace QANT.PRTG
{
    /// <summary>
    /// Channel
    /// </summary>
    public class Channel
    {
        private string _value = ((float)0.0).ToString(CultureInfo.InvariantCulture);
        private string _speedTime = PRTG.SpeedTime.Second;
        private string _mode = PRTG.Mode.Absolute;
        private string _float = "1";
        private string _decimalMode = PRTG.DecimalMode.Two;
        private string _warning = "0";
        private string _showChart = "1";
        private string _showTable = "1";
        private string _limitMode = "0";
        private string _unit = PRTG.Unit.Custom;
        private string _speedSize = SpeedOrVolumeSize.One;
        private string _volumeSize = SpeedOrVolumeSize.One;

        /// <summary>
        /// Size used for the display value. For example, if you have a value of 50000 and use Kilo as size the display is 50 kilo #. Default is One (value used as returned). For the Bytes and Speed units this is overridden by the setting in the user interface.
        /// </summary>
        public string VolumeSize
        {
            get { return _volumeSize; }
            set { _volumeSize = value; }
        }

        /// <summary>
        /// Size used for the display value. For example, if you have a value of 50000 and use Kilo as size the display is 50 kilo #. Default is One (value used as returned). For the Bytes and Speed units this is overridden by the setting in the user interface.
        /// </summary>
        public string SpeedSize
        {
            get { return _speedSize; }
            set { _speedSize = value; }
        }

        /// <summary>
        /// Define if you want to use a lookup file (e.g. to view integer values as status texts). Please enter the ID of the lookup file you want to use, or omit this element to not use lookups. Note: This setting will be considered only on the first sensor scan, when the channel is newly created; it is ignored on all further sensor scans (and may be omitted). You can change this initial setting later in the Channel settings of the sensor.
        /// </summary>
        public string ValueLookup { get; set; }

        /// <summary>
        /// Define if the limit settings defined above will be active. Default is 0 (no; limits inactive). If 0 is used the limits will be written to the sensor channel settings as predefined values, but limits will be disabled. Note: This setting will be considered only on the first sensor scan, when the channel is newly created; it is ignored on all further sensor scans (and may be omitted). You can change this initial setting later in the Channel settings of the sensor.
        /// </summary>
        public string LimitMode
        {
            get { return _limitMode; }
            set { _limitMode = value; }
        }

        /// <summary>
        /// Define an additional message. It will be added to the sensor's message when entering a "Warning" status that is triggered by a limit. Note: The values defined with this element will be considered only on the first sensor scan, when the channel is newly created; they are ignored on all further sensor scans (and may be omitted). You can change this initial setting later in the Channel settings of the sensor.
        /// </summary>
        public string LimitWarningMessage { get; set; }

        /// <summary>
        /// Define an additional message. It will be added to the sensor's message when entering a "Down" status that is triggered by a limit. Note: The values defined with this element will be considered only on the first sensor scan, when the channel is newly created; they are ignored on all further sensor scans (and may be omitted). You can change this initial setting later in the Channel settings of the sensor.
        /// </summary>
        public string LimitErrorMessage { get; set; }

        /// <summary>
        /// Define a lower error limit for the channel. If enabled, the sensor will be set to a "Down" status if this value is undercut and the LimitMode is activated. Note: Please provide the limit value in the unit of the base data type, just as used in the "Value" element of this section. While a sensor shows a "Down" status triggered by a limit, it will still receive data in its channels. The values defined with this element will be considered only on the first sensor scan, when the channel is newly created; They are ignored on all further sensor scans (and may be omitted). You can change this initial setting later in the Channel settings of the sensor.
        /// </summary>
        public string LimitMinError { get; set; }

        /// <summary>
        /// Define a lower warning limit for the channel. If enabled, the sensor will be set to a "Warning" status if this value is undercut and the LimitMode is activated. Note: Please provide the limit value in the unit of the base data type, just as used in the "Value" element of this section. The values defined with this element will be considered only on the first sensor scan, when the channel is newly created; they are ignored on all further sensor scans (and may be omitted). You can change this initial setting later in the Channel settings of the sensor.
        /// </summary>
        public string LimitMinWarning { get; set; }

        /// <summary>
        /// Define an upper warning limit for the channel. If enabled, the sensor will be set to a "Warning" status if this value is overrun and the LimitMode is activated. Note: Please provide the limit value in the unit of the base data type, just as used in the "Value" element of this section. The values defined with this element will be considered only on the first sensor scan, when the channel is newly created; they are ignored on all further sensor scans (and may be omitted). You can change this initial setting later in the Channel settings of the sensor.
        /// </summary>
        public string LimitMaxWarning { get; set; }

        /// <summary>
        /// Define an upper error limit for the channel. If enabled, the sensor will be set to a "Down" status if this value is overrun and the LimitMode is activated. Note: Please provide the limit value in the unit of the base data type, just as used in the "Value" element of this section. While a sensor shows a "Down" status triggered by a limit, it will still receive data in its channels. The values defined with this element will be considered only on the first sensor scan, when the channel is newly created; they are ignored on all further sensor scans (and may be omitted). You can change this initial setting later in the Channel settings of the sensor.
        /// </summary>
        public string LimitMaxError { get; set; }

        /// <summary>
        /// Init value for the Show in Table option. Default is 1 (yes). Note: The values defined with this element will be considered only on the first sensor scan, when the channel is newly created; they are ignored on all further sensor scans (and may be omitted). You can change this initial setting later in the Channel settings of the sensor.
        /// </summary>
        public string ShowTable
        {
            get { return _showTable; }
            set { _showTable = value; }
        }

        /// <summary>
        /// Init value for the Show in Graph option. Default is 1 (yes). Note: The values defined with this element will be considered only on the first sensor scan, when the channel is newly created; they are ignored on all further sensor scans (and may be omitted). You can change this initial setting later in the Channel settings of the sensor.
        /// </summary>
        public string ShowChart
        {
            get { return _showChart; }
            set { _showChart = value; }
        }

        /// <summary>
        /// If enabled for at least one channel, the entire sensor is set to warning status. Default is 0 (no).
        /// </summary>
        public string Warning
        {
            get { return _warning; }
            set { _warning = value; }
        }

        /// <summary>
        /// Init value for the Decimal Places option. If 0 is used in the "Float" element (i.e. use integer), the default is Auto; otherwise (i.e. for float) default is All. Note: You can change this initial setting later in the Channel settings of the sensor.
        /// </summary>
        public string DecimalMode
        {
            get { return _decimalMode; }
            set { _decimalMode = value; }
        }

        /// <summary>
        /// Define if the value is a float. Default is 0 (no). If set to 1 (yes), use a dot as decimal separator in values. Note: Define decimal places with the "DecimalMode" element.
        /// </summary>
        public string Float
        {
            get { return _float; }
            set { _float = value; }
        }

        /// <summary>
        /// Used when displaying the speed. Default is Second.
        /// </summary>
        public string SpeedTime
        {
            get { return _speedTime; }
            set { _speedTime = value; }
        }

        /// <summary>
        /// Selects if the value is a absolute value or counter. Default is Absolute.
        /// </summary>
        public string Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        /// <summary>
        /// 	If Custom is used as unit this is the text displayed behind the value.
        /// </summary>
        public string CustomUnit { get; set; }

        /// <summary>
        /// Name of the channel as displayed in user interfaces. This parameter is required and must be unique for the sensor.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The value as integer or float. Please make sure the "Float" setting matches the kind of value provided. Otherwise PRTG will show 0 values.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// The unit of the value. Default is Custom. Useful for PRTG to be able to convert volumes and times.
        /// </summary>
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }
    }
}