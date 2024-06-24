using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sys = Cosmos.System;

namespace DoorsOS
{


    public class Micro
    {
        static string contents = "";
        static string path = "";
        public static void welcome()
        {
            Console.Clear();
            Console.WriteLine("~");
            Console.WriteLine("~");
            Console.WriteLine("~");
            Console.WriteLine("~");
            Console.WriteLine("~                             micro - GEMS Text Editor");
            Console.WriteLine("~                      A simple UI for 2file, inspired by MIV.");
            Console.WriteLine("~                                  version 1.0");
            Console.WriteLine("~                                 by Samuel Lord");
            Console.WriteLine("~                                 License - MIT");
            Console.WriteLine("~");
            Console.WriteLine("~                      use q<enter>            to exit");
            Console.WriteLine("~                      use w<enter>            save to file");
            Console.WriteLine("~                      use c<enter>            to clear");
            Console.WriteLine("~                   type :$m<enter>            to use commands");
            Console.WriteLine("~");
            Console.WriteLine("~");
            Console.WriteLine("~");
            Console.WriteLine("~");
            Console.Write("~                                 Press any key to continue...");
            Console.ReadKey(true); //any key to continue
            Console.Clear();
            micro();


        }

        public static void startMicro(string pathb)
        {
            if (File.Exists(pathb))
            {
                microLoader(pathb);
                welcome();
            }
            else
            {
                welcome();
            }
        }

        public static void microLoader(string pathb)
        {
            //here we will open files and load them into the contents string.
            try
            {
                var hello_text = File.ReadAllText(pathb);
                contents = hello_text;
                path = pathb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static String micro()
        {
            var cmdMode = false;
            Console.Write("<**BEGIN READONLY**>");
            Console.Write(contents);
            Console.Write("<**END READONLY**>");
            while (true)
            {
                if (cmdMode == true)
                {
                    Console.Write("m>");
                    var cmd = Console.ReadLine();
                    if (cmd == "c")
                    {
                        contents = "";
                    }
                    else if (cmd == "q")
                    {
                        return null;
                    }
                    else if (cmd == "s")
                    {
                        File.WriteAllText(path, contents);
                    }
                    cmdMode = false;
                }
                var newline = Console.ReadLine();

                if (newline == ":$m")
                {
                    cmdMode = true;
                }

                if (cmdMode == false)
                {
                    contents += "\n" + newline;
                }
            }
        }




    }
}