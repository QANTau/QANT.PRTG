using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Net;

namespace QANT.PRTG.Eaton
{
    public static class Ups
    {
        private const string BaseOid = "1.3.6.1.2.1.33.1.";
        private const short Port = 161;

        private const short TestWarningDays = 30;
        private const short TestErrorDays = 60;

        public static void Check(CredentialsSnmp credentials)
        {
            // Populate the List of OID's to Get
            var asnItems = new Dictionary<string, SnmpAsnItem>
            {
                {BaseOid + "1.2.0", new SnmpAsnItem("Model")},
                {BaseOid + "1.3.0", new SnmpAsnItem("Firmware Version")},
                {BaseOid + "1.4.0", new SnmpAsnItem("Software Version")},
                {BaseOid + "1.5.0", new SnmpAsnItem("Serial Number")},
                {BaseOid + "6.1.0", new SnmpAsnItem("Alarms Present")},
                {BaseOid + "7.3.0", new SnmpAsnItem("Test Result Summary")},
                {BaseOid + "7.5.0", new SnmpAsnItem("Last Test Up Time")},

                { "1.3.6.1.2.1.1.3.0", new SnmpAsnItem("Up Time")},
                {"1.3.6.1.4.1.534.1.6.5.0", new SnmpAsnItem("Temperature")},
                {"1.3.6.1.4.1.534.1.6.6.0", new SnmpAsnItem("Humidity")}
            };

            var asnItemsLookup = new Dictionary<string, Vb>();

            // SNMP Query
            var snmpCommunity = new OctetString(credentials.Community);
            var agentParams = new AgentParameters(snmpCommunity)
            {
                Version = SnmpVersion.Ver2
            };

            var agent = new IpAddress(credentials.Host);
            var target = new UdpTarget((IPAddress)agent, Port, 2000, 1);

            var pdu = new Pdu(PduType.Get);
            foreach (var item in asnItems)
                pdu.VbList.Add(item.Key);

            var result = (SnmpV2Packet)target.Request(pdu, agentParams);
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

                var message = model;
                message += " SN " + asnItemsLookup["Serial Number"].Value;
                message += " (SW v" + asnItemsLookup["Software Version"].Value;
                message += " / FW v" + asnItemsLookup["Firmware Version"].Value + ")";

                var globalMessage = "";

                Channel channel;

                // Humidity
                // TODO - This is untested code
                var tempStr = asnItemsLookup["Humidity"].Value.ToString();
                if (!string.IsNullOrEmpty(tempStr))
                {
                    if (tempStr != "Null" && tempStr != "SNMP No-Such-Instance")
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
                // TODO - This is untested code
                tempStr = asnItemsLookup["Temperature"].Value.ToString();
                if (!string.IsNullOrEmpty(tempStr))
                {
                    if (tempStr != "Null" && tempStr != "SNMP No-Such-Instance")
                    {
                        var temp = Convert.ToInt32(tempStr); // / 10;  
                        //temp = (temp - 32) * 5 / 9; // (°F − 32) × 5/9 = °C
                        channel = new Channel
                        {
                            Description = "Temperature",
                            Unit = Unit.Temperature,
                            Value = temp.ToString()
                        };
                        output.Channels.Add(channel);
                    }
                }

                // Alarms Present
                var alarmsPresent = asnItemsLookup["Alarms Present"].Value.ToString();
                if (!string.IsNullOrEmpty(tempStr))
                {
                    var alarmsPresentWarning = "0";
                    if (alarmsPresent != "0")
                    {
                        alarmsPresentWarning = "1";
                    }
                    channel = new Channel
                    {
                        Description = "# Alarms Present",
                        Unit = Unit.Count,
                        Value = alarmsPresent,
                        Warning = alarmsPresentWarning
                    };
                    output.Channels.Add(channel);
                }

                // Test Results Summary
                var testResult = asnItemsLookup["Test Result Summary"].Value.ToString().Trim();
                var testResultWarning = "0";
                switch (testResult)
                {
                    case "1":
                        testResult = "donePass";
                        break;
                    case "2":
                        testResult = "doneWarning";
                        globalMessage = "[Test Warning]";
                        testResultWarning = "1";
                        break;
                    case "3":
                        testResult = "doneError";
                        globalMessage = "[Test Error]";
                        testResultWarning = "1";
                        break;
                    case "4":
                        testResult = "aborted";
                        globalMessage = "[Test Aborted]";
                        testResultWarning = "1";
                        break;
                    case "5":
                        testResult = "inProgress";
                        globalMessage = "[Test In Progress]";
                        break;
                    case "6":
                        testResult = "noTestsInitiated";
                        globalMessage = "[Test Never Initiated]";
                        testResultWarning = "1";
                        break;
                    default:
                        testResult = "unknown";
                        globalMessage = "[Test Result Unknown]";
                        testResultWarning = "1";
                        break;
                }
                channel = new Channel
                {
                    Description = "UPS Test Result",
                    Unit = Unit.Custom,
                    CustomUnit = "Result",
                    Value = testResult,
                    Warning = testResultWarning,
                    
                };
                output.Channels.Add(channel);

                // Last Test Date
                var timeSinceBoot = (TimeTicks)asnItemsLookup["Up Time"].Value;
                var timeSinceTest = (TimeTicks)asnItemsLookup["Last Test Up Time"].Value;
                var relativeTimeSinceTest = timeSinceBoot.Milliseconds - timeSinceTest.Milliseconds;
                var lastTestDate = DateTime.Now - TimeSpan.FromMilliseconds(relativeTimeSinceTest);

                var daysSinceBatteryTest = Convert.ToInt16((DateTime.Now - lastTestDate).TotalDays);
                channel = new Channel
                {
                    Description = "Days Since UPS Test",
                    Unit = Unit.Custom,
                    CustomUnit = "Days",
                    LimitErrorMessage = "UPS Test Overdue",
                    LimitWarningMessage = "UPS Test Required",
                    LimitMaxWarning = TestWarningDays.ToString(),
                    LimitMaxError = TestErrorDays.ToString(),
                    LimitMode = "1",
                    Value = daysSinceBatteryTest.ToString()
                };
                output.Channels.Add(channel);

                if (!string.IsNullOrEmpty(globalMessage))
                {
                    message += " " + globalMessage;
                }
                output.Text = message;
                output.WriteOutput();

            }
            else
            {
                Error.WriteOutput("No Results Received from " + credentials.Host + " using community " + credentials.Community);
            }

        }

    }
}
