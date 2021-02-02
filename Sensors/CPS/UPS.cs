using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Net;

namespace QANT.PRTG.CPS
{
    public static class Ups
    {
        private const string BaseOid = "1.3.6.1.4.1.3808.1.1.";
        private const short Port = 161;

        private const short BatteryTestWarningDays = 30;
        private const short BatteryTestErrorDays = 60;

        private const short BatteryCalibrationWarningDays = 180;
        private const short BatteryCalibrationErrorDays = 270;

        public static void Check(CredentialsSnmp credentials)
        {
            // Populate the List of OID's to Get
            var asnItems = new Dictionary<string, SnmpAsnItem>
            {
                {BaseOid + "1.1.2.4.0", new SnmpAsnItem("Software Version")},
                {BaseOid + "1.1.2.1.0", new SnmpAsnItem("Firmware Version")},
                {BaseOid + "1.1.1.1.0", new SnmpAsnItem("Model")},
                {BaseOid + "1.2.1.3.0", new SnmpAsnItem("Battery Replacement Date")},
                {BaseOid + "1.7.2.4.0", new SnmpAsnItem("Battery Test Date")},
                {BaseOid + "1.7.2.3.0", new SnmpAsnItem("Battery Test Result")},
                {BaseOid + "1.7.2.8.0", new SnmpAsnItem("Battery Calibration Date")},
                {BaseOid + "1.7.2.7.0", new SnmpAsnItem("Battery Calibration Result")},
                {BaseOid + "4.3.1.0", new SnmpAsnItem("Humidity")},
                {BaseOid + "4.2.1.0", new SnmpAsnItem("Temperature")},
                {BaseOid + "1.1.2.3.0", new SnmpAsnItem("Serial Number")}
            };

            var asnItemsLookup = new Dictionary<string, Vb>();

            // SNMP Query
            var snmpCommunity = new OctetString(credentials.Community);
            var agentParams = new AgentParameters(snmpCommunity)
            {
                Version = SnmpVersion.Ver1
            };

            var agent = new IpAddress(credentials.Host);
            var target = new UdpTarget((IPAddress)agent, Port, 2000, 1);

            var pdu = new Pdu(PduType.Get);
            foreach (var item in asnItems)
                pdu.VbList.Add(item.Key);

            var result = (SnmpV1Packet)target.Request(pdu, agentParams);
            if (result != null)
            {
                // Populate Results
                foreach (var item in result.Pdu.VbList)
                    if (asnItems.ContainsKey(item.Oid.ToString()))
                        asnItems[item.Oid.ToString()].Vb = item;

                // Build Reverse Lookup Dictionary
                foreach (var item in asnItems)
                    asnItemsLookup.Add(item.Value.Description, item.Value.Vb);

                var output = new Result();
                var model = asnItemsLookup["Model"].Value.ToString();

                var message = "CyberPower " + model;
                if (!string.IsNullOrEmpty(asnItemsLookup["Serial Number"].Value.ToString()))
                {
                    message += " SN " + asnItemsLookup["Serial Number"].Value;
                }
                message += " (SW v" + asnItemsLookup["Software Version"].Value;
                message += " / FW v" + asnItemsLookup["Firmware Version"].Value + ")";

                Channel channel;

                // Humidity
                var tempStr = asnItemsLookup["Humidity"].Value.ToString();
                if (!string.IsNullOrEmpty(tempStr))
                {
                    if (tempStr != "Null")
                    {
                        channel = new Channel
                        {
                            Description = "Humidity",
                            Unit = Unit.Percent,
                            Value = asnItemsLookup["Humidity"].Value.ToString()
                        };
                        output.Channels.Add(channel);
                    }
                }

                // Temperature
                tempStr = asnItemsLookup["Temperature"].Value.ToString();
                if (!string.IsNullOrEmpty(tempStr))
                {
                    if (tempStr != "Null")
                    {
                        var temp = Convert.ToInt32(tempStr) / 10;
                        temp = (temp - 32) * 5 / 9; // (°F − 32) × 5/9 = °C
                        channel = new Channel
                        {
                            Description = "Temperature",
                            Unit = Unit.Temperature,
                            Value = temp.ToString()
                        };
                        output.Channels.Add(channel);
                    }
                }

                // Battery Test Result
                var batteryTestResult = asnItemsLookup["Battery Test Result"].Value.ToString().Trim();
                var batteryTestWarning = "0";
                if (batteryTestResult != "1")
                {
                    batteryTestResult = "FAILURE";
                    batteryTestWarning = "1";
                }
                channel = new Channel
                {
                    Description = "Battery Test Result",
                    Unit = Unit.Custom,
                    CustomUnit = "Result",
                    Value = batteryTestResult,
                    Warning = batteryTestWarning
                };
                output.Channels.Add(channel);

                // Battery Calibration Result
                var batteryCalibrationResult = asnItemsLookup["Battery Calibration Result"].Value.ToString().Trim();
                var batteryCalibrationWarning = "0";
                if (batteryCalibrationResult != "1")
                {
                    batteryCalibrationResult = "FAILURE";
                    batteryCalibrationWarning = "1";
                }
                channel = new Channel
                {
                    Description = "Battery Calibration Result",
                    Unit = Unit.Custom,
                    CustomUnit = "Result",
                    Value = batteryCalibrationResult,
                    Warning = batteryCalibrationWarning
                };
                output.Channels.Add(channel);

                // Battery Test Date
                DateTime batteryTestDate;
                var batteryTestDateStr = CheckDateTimeFormat(asnItemsLookup["Battery Test Date"].Value.ToString(), model);
                if (string.IsNullOrEmpty(batteryTestDateStr) || batteryTestDateStr.ToLower() == "null")
                    batteryTestDate = new DateTime(2010, 01, 01);
#if OLDCODE
                else
                {
                    if (model == "OL1500ERTXL2U")
                        batteryTestDate = DateTime.ParseExact(batteryTestDateStr, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    else
                        batteryTestDate = DateTime.Parse(batteryTestDateStr);
                }
#else
                else
                    batteryTestDate = DateTime.Parse(batteryTestDateStr);
#endif
                var daysSinceBatteryTest = Convert.ToInt16((DateTime.Now - batteryTestDate).TotalDays);
                channel = new Channel
                {
                    Description = "Days Since Battery Test",
                    Unit = Unit.Custom,
                    CustomUnit = "Days",
                    LimitErrorMessage = "Battery Test Overdue",
                    LimitWarningMessage = "Battery Test Required",
                    LimitMaxWarning = BatteryTestWarningDays.ToString(),
                    LimitMaxError = BatteryTestErrorDays.ToString(),
                    LimitMode = "1", 
                    Value = daysSinceBatteryTest.ToString()
                };
                output.Channels.Add(channel);

                // Battery Calibration Date
                DateTime batteryCalibrationDate;
                var batteryCalibrationCalibration = CheckDateTimeFormat(asnItemsLookup["Battery Calibration Date"].Value.ToString(), model);
                if (string.IsNullOrEmpty(batteryCalibrationCalibration) || batteryCalibrationCalibration.ToLower() == "null")
                    batteryCalibrationDate = new DateTime(2010, 01, 01);
                else
                    batteryCalibrationDate = DateTime.Parse(batteryCalibrationCalibration);
                var daysSinceBatteryCalibration = Convert.ToInt16((DateTime.Now - batteryCalibrationDate).TotalDays);
                channel = new Channel
                {
                    Description = "Days Since Battery Calibration",
                    Unit = Unit.Custom,
                    CustomUnit = "Days",
                    LimitErrorMessage = "Battery Calibration Overdue",
                    LimitWarningMessage = "Battery Calibration Required",
                    LimitMaxWarning = BatteryCalibrationWarningDays.ToString(),
                    LimitMaxError = BatteryCalibrationErrorDays.ToString(),
                    LimitMode = "1",
                    Value = daysSinceBatteryCalibration.ToString()
                };
                output.Channels.Add(channel);

                output.Text = message;
                output.WriteOutput();

            }
            else
            {
                Error.WriteOutput("No Results Received from " + credentials.Host + " using community " + credentials.Community);
            }

        }

        private static string CheckDateTimeFormat(string inputDt, string model)
        {
#if OLDCODE
            if (model == "OL1500ERTXL2U" || model == "OL3000ERTXL2U")
            {
                if (inputDt.Length != 10) return inputDt;
                var month = inputDt.Substring(0, 2);
                var day = inputDt.Substring(3, 2);
                var year = inputDt.Substring(6, 4);
                return day + "/" + month + "/" + year;
            }
            else
            {
                Error.WriteOutput("Unknown model: " + model);
                System.Windows.Forms.Application.Exit();
            }
#else
            try
            {
                if (inputDt.Length != 10) return inputDt;
                var month = inputDt.Substring(0, 2);
                var day = inputDt.Substring(3, 2);
                var year = inputDt.Substring(6, 4);
                return day + "/" + month + "/" + year;
            }
            catch (Exception ex)
            {
                Error.WriteOutput("Application Exception: " + ex.Message);
                System.Windows.Forms.Application.Exit();
            }
#endif
            return inputDt;
        }

    }
}
