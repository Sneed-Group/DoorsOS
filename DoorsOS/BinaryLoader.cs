using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace DoorsOS
{
    internal class BinaryLoader
    {
        public static bool LoadGemsBin(string path)
        {
            try
            {
                if (path.EndsWith(".bin"))
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(bytes);
                    return true;
                }
                else
                {
                    Console.WriteLine("Not a .bin file");
                    return false;
                }
            }
            catch
            {
                Console.WriteLine("Generic error. (Too big?)");
                return false;
            }
        }
    }
}
