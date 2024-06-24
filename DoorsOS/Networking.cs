using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Cosmos.HAL;
using Cosmos.HAL.Drivers;
using Cosmos.HAL.Drivers.PCI.Network;
using Cosmos.HAL.Network;

namespace DoorsOS
{
    public class Networking
    {
        public static string mac;
        public static PCIDevice dev;
        private static PCIDevice dev2;
        private static string dev3;
        public static bool netAvail = false;
        public static AMDPCNetII nic;
        public static string recievedPackets;
        public static bool isNetworkingAvailable()
        {
            dev = PCI.GetDevice(VendorID.AMD, DeviceID.PCNETII);
            if (dev != null)
            {
                netAvail = true;
                return true;
            }
            else
            {
                netAvail = false;
                return false;
            }
        }

        public static string GetMACAddress()
        {
            if (netAvail)
            {
                AMDPCNetII nic = new AMDPCNetII(dev);
                mac = nic.MACAddress.ToString();
                return nic.MACAddress.ToString();
            }
            else
            {
                return "Fail. (Are you sure you have an AMD PCnet-PCI II installed?)";
            }
        }

        public static string getNetReady()
        {
            if (netAvail)
            {
                nic = new AMDPCNetII(dev);
                mac = nic.MACAddress.ToString();
                nic.Enable();
                return "Ready";
            }
            else
            {
                return "Fail. (Are you sure you have an AMD PCnet-PCI II installed?)";
            }
        }

        public static string sendBytes(byte[] packetBuffer)
        {
            if (nic.Ready)
            {
                if (nic.IsSendBufferFull())
                {
                    return "Uh oh! The send buffer is full!";
                }
                else
                {
                    var a = nic.QueueBytes(packetBuffer);
                    a.ToString();
                    return "Bytes buffered: " + a;
                }
            }
            else
            {
                return "Fail.";
            }
        }

        public static string recieveBytes(int offset, int max)
        {
            if (nic.Ready)
            {
                byte[] dataBytes1 = Encoding.ASCII.GetBytes("");
                nic.ReceiveBytes(dataBytes1, offset, max);
                var data = nic.DataReceived.ToString();
                byte[] dataBytes2 = Encoding.ASCII.GetBytes(data);
                return "Recieved as bytes: " + data;
            }
            else
            {
                return "Fail.";
            }
        }

        public static string recievePackets(int offset, int max)
        {
            if (nic.Ready)
            {
                var dataPacket = nic.ReceivePacket();
                return "Recieved as packet: " + dataPacket.ToString();
            }
            else
            {
                return "Fail.";
            }
        }
    }
}