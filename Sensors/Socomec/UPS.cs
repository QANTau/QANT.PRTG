using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Net;

namespace QANT.PRTG.Socomec
{
    public static class Ups
    {
        private const string BaseOid = "1.3.6.1.4.1.4555.1.1.7.";
        private const short Port = 161;

        public static void Check(CredentialsSnmp credentials)
        {
            // Populate the List of OID's to Get
            var asnItems = new Dictionary<string, SnmpAsnItem>
            {
                {BaseOid + "1.1.1.0", new SnmpAsnItem("Model")},
                {BaseOid + "1.1.5.0", new SnmpAsnItem("Software Version")},
                {BaseOid + "1.1.2.0", new SnmpAsnItem("Serial Number")},
                {BaseOid + "1.6.1.0", new SnmpAsnItem("Alarms Present")},
                {BaseOid + "1.10.1.0", new SnmpAsnItem("Temperature")},
                {BaseOid + "1.10.2.0", new SnmpAsnItem("Humidity")}
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
                            Value = humidity.ToString()
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
