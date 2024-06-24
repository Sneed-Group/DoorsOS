using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DoorsOS
{
    internal class WDCL
    {
        public static void launchExeOld(byte bin)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(bin);
        }

        public static IDictionary<int, string> openFiles = new Dictionary<int, string>();
        public static void launchExe(byte[] bin)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(bin);
            while (true)
            {
                var callzz = BitConverter.ToString(memoryStream.GetBuffer());
                twentyoneHandler(callzz);
            }
        }
        static int ofi = 2;
        public static string twentyoneHandler(string callz)
        {
            var fs = File.OpenWrite("0:\\root");
            string[] callzArray = callz.Split(" ");
            if (callz.Contains("00h"))
            {
                Cosmos.System.Power.Reboot();
                return "0";
            }
            else if (callz.Contains("01h"))
            {
                string a = Console.ReadLine();
                return a;
            }
            else if (callz.Contains("02h"))
            {
                string ostr = "";
                for (var i = 0; i < callzArray.Length; i++)
                {
                    if (callzArray[i] == "02h")
                    {
                        ostr += callzArray[i + 1];
                        Console.Write(callzArray[i + 1]);
                    }
                }
                return ostr;
            }
            else if (callz.Contains("09h"))
            {
                string str = "";
                for (var i = 0; i < callzArray.Length; i++)
                {
                    if (callzArray[i] == "09h")
                    {
                        str += callzArray[i + 1];
                        Console.Write(callzArray[i + 1]);
                    }
                }
                return str;
            }
            else if (callz.Contains("19h"))
            {
                return "0:\\";
            }
            else if (callz.Contains("0Fh"))
            {
                var c = callz.ToString();
                var cs = c.Split("0fh");
                fs = File.OpenWrite(cs[ofi]);
                openFiles.Add(ofi, cs[ofi]);
                ofi++;
                return cs[ofi];
            }
            else if (callz.Contains("10h"))
            {
                var c = callz.ToString();
                var cs = c.Split("0fh");
                var towrite = c.Split("10h");
                File.AppendAllText(openFiles[ofi], towrite.ToString());
                ofi++;
                return cs[ofi];
            }
            else
            {
                return "0";
            }



        }
    }
}
