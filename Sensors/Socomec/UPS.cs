using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace QANT.PRTG.Socomec
{
    public static class Ups
    {
        private const string BaseOid = "1.3.6.1.4.1.4555.1.1.7.1.";
        private const short Port = 161;

        public static void Check(CredentialsSnmp credentials)
        {
            // Populate the List of OID's to Get
            var asnItems = new Dictionary<string, SnmpAsnItem>
            {
                {BaseOid + "1.1.0", new SnmpAsnItem("Model")},
                {BaseOid + "1.2.0", new SnmpAsnItem("Serial Number")},
                {BaseOid + "1.5.0", new SnmpAsnItem("Software Version")},
                {BaseOid + "2.1.0", new SnmpAsnItem("Battery Status")},
                {BaseOid + "2.3.0", new SnmpAsnItem("Estimated Runtime")},
                {BaseOid + "6.1.0", new SnmpAsnItem("Alarms Present")},
                {BaseOid + "7.1.0", new SnmpAsnItem("UPS Status")},
                {BaseOid + "10.1.0", new SnmpAsnItem("Temperature")},
                {BaseOid + "10.2.0", new SnmpAsnItem("Humidity")}
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

            var result = new SnmpV2Packet();
            try
            {
                result = (SnmpV2Packet)target.Request(pdu, agentParams);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Request has reached maximum retries.")
                {
                    Error.WriteOutput("SNMP Request to " + credentials.Host + " with community " + credentials.Community + " has exceeded the maximum number of retries.");
                    return;
                }
            }

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
                message += " (SW v" + asnItemsLookup["Software Version"].Value + ")";

                Channel channel;

                // Humidity
                var tempStr = asnItemsLookup["Humidity"].Value.ToString();
                if (!string.IsNullOrEmpty(tempStr))
                {
                    if (tempStr != "Null" && tempStr != "SNMP No-Such-Instance")
                    {
                        var humidity = Convert.ToInt32(tempStr) / 10;
                        channel = new Channel
                        {
                            Description = "Humidity",
                            Unit = Unit.Percent,
                            Value = humidity.ToString(),
                            LimitMinError = "5",
                            LimitMinWarning = "10",
                            LimitMaxWarning = "90",
                            LimitMaxError = "95",
                            LimitMode = "1"
                        };
                        output.Channels.Add(channel);
                    }
                }

                // Temperature
                tempStr = asnItemsLookup["Temperature"].Value.ToString();
                if (!string.IsNullOrEmpty(tempStr))
                {
                    if (tempStr != "Null" && tempStr != "SNMP No-Such-Instance")
                    {
                        var temp = Convert.ToInt32(tempStr) / 10;
                        channel = new Channel
                        {
                            Description = "Temperature",
                            Unit = Unit.Temperature,
                            Value = temp.ToString(),
                            LimitMinError = "10",
                            LimitMinWarning = "15",
                            LimitMaxWarning = "50",
                            LimitMaxError = "55",
                            LimitMode = "1"
                        };
                        output.Channels.Add(channel);
                    }
                }

                // UPS Status
                tempStr = asnItemsLookup["UPS Status"].Value.ToString();
                if (!string.IsNullOrEmpty(tempStr))
                {
                    channel = new Channel
                    {
                        Description = "UPS Status",
                        Unit = Unit.Custom,
                        Value = tempStr
                    };

                    switch (Convert.ToInt32(tempStr))
                    {
                        case 1:
                            channel.Warning = "1";
                            message += " Standby On";
                            output.Error = true;
                            break;
                        case 2:
                            channel.Warning = "1";
                            message += " Standby Off";
                            output.Error = true;
                            break;
                        case 3:
                            message += " Eco Mode";
                            break;
                        case 4:
                            break;
                        case 5:
                            channel.Warning = "1";
                            message += " Alarm Reset";
                            break;
                        case 6:
                            channel.Warning = "1";
                            message += " On Bypass";
                            break;
                        case 7:
                            channel.Warning = "1";
                            message += " On Inverter";
                            break;
                        default:
                            channel.Warning = "1";
                            message += " Unknown Status";
                            output.Error = true;
                            break;
                    }

                    output.Channels.Add(channel);
                }

                // Battery Status
                tempStr = asnItemsLookup["Battery Status"].Value.ToString();
                if (!string.IsNullOrEmpty(tempStr))
                {
                    channel = new Channel
                    {
                        Description = "Battery Status",
                        Unit = Unit.Custom,
                        Value = tempStr
                    };

                    switch (Convert.ToInt32(tempStr))
                    {
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            message += " [Battery Test]";
                            channel.Warning = "1";
                            break;
                        case 5:
                            message += " [Battery Discharging]";
                            channel.Warning = "1";
                            break;
                        case 6:
                            message += " [Battery Low]";
                            channel.Warning = "1";
                            output.Error = true;
                            break;
                        case 7:
                            message += " [Battery Depleted]";
                            channel.Warning = "1";
                            output.Error = true;
                            break;
                        case 8:
                            message += " [Battery Failure]";
                            channel.Warning = "1";
                            output.Error = true;
                            break;
                        case 9:
                            message += " [Battery Disconnected]";
                            channel.Warning = "1";
                            output.Error = true;
                            break;
                        default:
                            message += " [Battery Status Unknown]";
                            channel.Warning = "1";
                            break;
                    }

                    output.Channels.Add(channel);
                }

                // Estimated Runtime
                tempStr = asnItemsLookup["Estimated Runtime"].Value.ToString();
                if (!string.IsNullOrEmpty(tempStr))
                {
                    if (tempStr != "Null" && tempStr != "SNMP No-Such-Instance")
                    {
                        var minutes = Convert.ToInt32(tempStr);
                        if (minutes > 120)
                            minutes = 120;
                        channel = new Channel
                        {
                            Description = "Estimated Runtime",
                            Unit = Unit.Count,
                            CustomUnit = "minutes",
                            Value = minutes.ToString()
                        };
                        if (minutes < 15)
                        {
                            output.Error = true;
                            channel.Warning = "1";
                            message += " < 15mins Runtime";
                        }
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
