using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using System.IO;
using static DoorsOS.Networking;
using static DoorsOS.Wait;
using cc = DoorsOS.KernelColors;
using System.ComponentModel;
using Cosmos.System.Graphics;
using System.Drawing;
using mouse = Cosmos.System.MouseManager;
using System.Threading;
using Cosmos.HAL.Network;
using System.Diagnostics;
using Cosmos.System.Network.IPv4;
using System.Linq;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using static DoorsOS.WDCL;
using DoorsOS;

namespace DoorsOS
{
    public class Kernel : Sys.Kernel
    {
        string versionSTR = "2024.6.24.0";

        Sys.FileSystem.CosmosVFS fs;

        public string current_directory = "0:\\";
        string currDir = "";
        public static string file;
        public int rWI = 0;

        protected override void BeforeRun()
        {
            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Clear();

        }


        public void hackyCarolFix()
        {
            Random rW = new Random(); //random Word
            rWI = rW.Next(0, 12); //pick a word
        }
        public void dir()
        {
            try
            {
                string[] dirs = Directory.GetDirectories(current_directory);
                string[] files = Directory.GetFiles(current_directory);
                Console.WriteLine("--DIRS/FILES--");
                foreach (var item in dirs)
                {
                    Console.WriteLine("d> " + item);
                }
                foreach (var item in files)
                {
                    Console.WriteLine("f> " + item);
                }
                Console.WriteLine("--------------");
            }
            catch
            {
                Console.WriteLine("You are in an invalid directory!");
                Console.WriteLine("Try \" cd 0:\\\\ \"?");
            }
        }

        public void mkdir(string dir2make)
        {
            try
            {
                fs.CreateDirectory(dir2make);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void cd(string dirCD)
        {
            if (fs.IsValidDriveId(dirCD.Replace(":\\", "")) || fs.IsValidDriveId(dirCD.Replace(":", "")) || fs.IsValidDriveId(dirCD))
            {
                try
                {
                    current_directory = dirCD;
                }
                catch
                {
                    try
                    {
                        Console.WriteLine("!!! Error. Going to root NOW. !!!");
                        current_directory = "0:\\";
                    }
                    catch
                    {
                        Console.WriteLine("!!! Dir and root not found. !!!");
                    }
                }
            }
            else
            {
                if (dirCD == "..")
                {
                    try
                    {
                        string prevDirTemp = current_directory.ToString().Replace(currDir.ToString() + "\\", "\\");
                        Console.WriteLine(prevDirTemp.ToString());
                        current_directory = prevDirTemp.ToString();
                        currDir = fs.GetDirectory(prevDirTemp).mName.ToString();
                    }
                    catch
                    {
                        Console.WriteLine("!!! Directory not found. !!!");
                    }
                }
                else
                {
                    currDir = dirCD.ToString();
                    if (fs.GetDirectory(current_directory + dirCD) != null)
                    {
                        try
                        {
                            current_directory = current_directory + dirCD + "\\";
                        }
                        catch
                        {
                            Console.WriteLine("!!! Directory not found. !!!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("!!! Directory not found. !!!");
                    }
                }
            }
        }

        public void del(string fileOrDir)
        {
            try
            {
                fs.DeleteDirectory(fs.GetDirectory(fileOrDir));
            }
            catch (Exception e)
            {
                try
                {
                    File.Delete(fileOrDir);
                }
                catch
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public string readFile(string dogFile)
        {
            try
            {
                var hello_file = fs.GetFile(current_directory + dogFile);
                var hello_file_stream = hello_file.GetFileStream();

                if (hello_file_stream.CanRead)
                {
                    byte[] text_to_read = new byte[hello_file_stream.Length];
                    hello_file_stream.Read(text_to_read, 0, (int)hello_file_stream.Length);
                    return Encoding.Default.GetString(text_to_read);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return "";
        }

        public void listDisk()
        {
            foreach (var item in fs.GetVolumes())
            {
                Console.WriteLine(item);
            }
        }

        public string writeLineToFile(string echoFile, string writeContents)
        {
            try
            {
                using (StreamWriter w = File.AppendText(current_directory + echoFile))
                {
                    return writeContents;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return "";
        }

        Canvas canvas;
        public void initGUI()
        {
            canvas = FullScreenCanvas.GetFullScreenCanvas();

            canvas.Clear(Color.Blue);

            mouse.ScreenWidth = Convert.ToUInt32(640);
            mouse.ScreenHeight = Convert.ToUInt32(480);
        }

        public void clearGUI(System.Drawing.Color color)
        {
            canvas.Clear(color);
        }

        public void createCursor(System.Drawing.Color color)
        {
            while (true)
            {
                try
                {
                    Pen pen = new Pen(color);
                    canvas.DrawRectangle(pen, mouse.X, mouse.Y, mouse.X + 20, mouse.Y - 10);
                }
                catch
                {
                    Console.WriteLine("Error making cursor.");
                }
            }
        }

        public bool isClicked(int x1, int y1, int x2, int y2, Sys.MouseState state)
        {
            while (true)
            {
                //Example of state: Sys.MouseState.Left
                if (mouse.MouseState == state && mouse.X < x1 && mouse.X > x2 && mouse.Y < y1 && mouse.Y > y2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



        public void drawElement(string type, int x1, int y1, int x2, int y2, System.Drawing.Color color)
        {
            Pen pen = new Pen(color);
            if (type == "line")
            {
                try
                {
                    canvas.DrawLine(pen, x1, y1, x2, y2);
                }
                catch
                {
                    Console.WriteLine("Error drawing line. Unknown error.");
                }
            }
            else if (type == "rectangle")
            {
                try
                {
                    canvas.DrawRectangle(pen, x1, y1, x2, y2);
                }
                catch
                {
                    Console.WriteLine("Error rectangle line. Unknown error.");
                }
            }
            else
            {
                Console.WriteLine("Error drawing. Unknown type.");
            }
        }

        public void setRes(int width, int height, ColorDepth colordepth)
        {
            canvas.Mode = new Mode(width, height, ColorDepth.ColorDepth32); //COSMOS only supports 32bit color depth. This is why we must use it.
            //Also, we can *try* higher resolutions, but COSMOS community recommends sticking with 800x600 and 640x480.
        }

        public void drawImageFromBase64(string base64file, int x, int y)
        {
            Bitmap bmp = new Bitmap(Convert.FromBase64String(base64file));

            Image img = (Image)bmp;

            canvas.DrawImage(img, x, y);
        }

        protected override void Run()
        {
            console.beep(0);
            Console.Clear();
            Console.WriteLine("Welcome to DoorsOS!");
            //Console.WriteLine(@"Now I can code anywhere
            //I go! Sweet!");
            while (1 == 1)
            {
                Console.Write(current_directory + " $> ");
                var cmd = Console.ReadLine();
                var args = cmd.Split(' ');
                //Console.Write("*RUSHELL DEBUG* Command typed: ");
                //Console.WriteLine(cmd);
                if (cmd == "clear")
                {
                    Console.Clear();
                }
                else if (cmd == "lose")
                {
                    initGUI();
                    drawElement("rectangle", 0, 0, 50, 50, Color.Red);

                    createCursor(Color.Green);
                    if (isClicked(0, 0, 50, 50, Sys.MouseState.Left))
                    {
                        clearGUI(Color.Blue);
                    }
                }
                else if (cmd == "reboot")
                {
                    Console.Clear();
                    Console.WriteLine("System now rebooting...");
                    Cosmos.System.Power.Reboot();
                }
                else if (cmd.StartsWith("micro"))
                {
                    var path = args[1];
                    Micro.startMicro(path);
                }
                else if (cmd == "shutdown")
                {
                    Console.Clear();
                    Console.WriteLine("System now shutting down...");
                    Cosmos.System.Power.Shutdown();
                }
                else if (cmd == "time")
                {
                    var time = Cosmos.HAL.RTC.Hour.ToString() + ":" + Cosmos.HAL.RTC.Minute.ToString() + ":" + Cosmos.HAL.RTC.Second.ToString();
                    Console.WriteLine(time);
                }
                else if (cmd == "date")
                {
                    var time = Cosmos.HAL.RTC.Month.ToString() + "-" + Cosmos.HAL.RTC.DayOfTheMonth.ToString() + "-" + Cosmos.HAL.RTC.Year.ToString();
                    Console.WriteLine(time);
                }
                else if (cmd == "about")
                {
                    long available_space = fs.GetAvailableFreeSpace("0:/");
                    string fs_type = fs.GetFileSystemType("0:/");
                    Console.WriteLine("DoorsOS Version: " + versionSTR);
                    try
                    {
                        Console.WriteLine("File System Type on main drive: " + fs_type);
                        Console.WriteLine("Available Free Space on main drive: " + available_space);
                    }
                    catch
                    {
                        Console.WriteLine("File System Type on main drive: NONE");
                        Console.WriteLine("Available Free Space on main drive: 0");
                    }
                }
                else if (cmd == "dir")
                {
                    dir();
                }
                else if (cmd == "drives")
                {
                    Console.WriteLine("Drives:");
                    Console.WriteLine("---");
                    for (int i = 0; i < 100; i++)
                    {
                        var possibleDrive = Cosmos.Common.StringHelper.GetNumberString(i) + ":\\";
                        if (fs.IsValidDriveId(possibleDrive)) //wow ok zoomer
                        {
                            try
                            {
                                Console.WriteLine(" Free space:" + fs.GetAvailableFreeSpace(possibleDrive));
                                Console.WriteLine(" Total space:" + fs.GetTotalFreeSpace(possibleDrive));
                                Console.WriteLine(" Drive ID: " + possibleDrive);
                                Console.WriteLine("---");
                            }
                            catch { }
                        }
                    }
                }

                else if (cmd.ToString().StartsWith("echo"))
                {
                    var echoing = cmd.ToString().Remove(0, 5);
                    Console.WriteLine(echoing);
                }
                else if (cmd.ToString().StartsWith("execWdcl"))
                {
                    try
                    {
                        WDCL.launchExe(File.ReadAllBytes(args[1]));
                    }
                    catch
                    {
                        Console.WriteLine("error!");
                    }
                }
                else if (cmd.ToString().StartsWith("execBin"))
                {
                    try
                    {
                        BinaryLoader.LoadGemsBin(args[1]);
                    }
                    catch
                    {
                        Console.WriteLine("error!");
                    }
                }
                else if (cmd.ToString().StartsWith("cd"))
                {
                    var dirCD = cmd.ToString().Remove(0, 3);
                    cd(dirCD);
                }
                else if (cmd.ToString().StartsWith("setvol"))
                {
                    cd(args[1]);
                }
                else if (cmd.ToString().StartsWith("listvol"))
                {
                    listDisk();
                }
                else if (cmd.ToString().StartsWith("del"))
                {
                    del(args[1]);
                }
                else if (cmd.ToString().StartsWith("deldir"))
                {
                    del(args[1]);
                }
                else if (cmd.ToString().StartsWith("2file"))
                {
                    var writeContents = cmd.ToString().Remove(0, 6);
                    Console.Write("Filename> ");
                    var echoFile = Console.ReadLine();
                    writeLineToFile(echoFile, writeContents);
                }
                else if (cmd.ToString().StartsWith("mkdir"))
                {
                    var makeDir = cmd.ToString().Remove(0, 6);
                    mkdir(makeDir);
                }
                else if (cmd.ToString().StartsWith("mkfile"))
                {
                    var makeFile = cmd.ToString().Remove(0, 7);
                    try
                    {
                        File.Create(current_directory + makeFile);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                else if (cmd.ToString() == "getMACAddr")
                {
                    Console.WriteLine(Networking.GetMACAddress());
                }
                else if (cmd.ToString() == "netAvailable")
                {
                    Console.WriteLine(Networking.isNetworkingAvailable().ToString());
                }
                else if (cmd.ToString() == "sOracle")
                {
                    string oracle = "sOracle says:";
                    string[] oracleWords = { "has", "forgive", "god", "jesus", "santa", "oracle", "forgiven", "bad", "good", "naughty", "samuel", "toys", "excellent", "tech", "husband", "thou", "you", "programmer", "linux", "unix", "posix", "dinosaurs", "david", "femboys", "gays" };
                    Random rand = new Random(); //Random Number of Words
                    Random randWords = new Random(); //Random Number of Words
                    int numwords = rand.Next(1, 42);
                    for (int i = 0; i < numwords; i++) //repeat for each word
                    {
                        numwords = rand.Next(1, 42);
                        string randWordStr = oracleWords[randWords.Next(0, oracleWords.Length - 1)];
                        hackyCarolFix(); //hacky fix because for some reason the OS seems to only set the word once, but prints it multiple times.
                        oracle += " " + randWordStr; //add word
                    }
                    Console.WriteLine(oracle);
                }
                else if (cmd.ToString().StartsWith("color"))
                {
                    try
                    {
                        var fg = args[1].ToString();
                        var bg = args[2].ToString();
                        if (fg == bg)
                        {
                            Console.WriteLine("Foreground and background can't be equal!");
                        }
                        else if (fg == "help")
                        {
                            Console.WriteLine(@"Colors:
            Black
            (Dark)Gray 
            (Dark)Blue
            (Dark)Green
            (Dark)Cyan
            (Dark)Red
            (Dark)Magenta
            (Dark)Yellow
            White");
                        }
                        else
                        {
                            Console.WriteLine("Will be available once we fix this.");
                            Console.WriteLine("(Not sure if it's me or Cosmos.)");
                            //Console.ForegroundColor = (ConsoleColor)cc.getColor(fg);
                            //Console.BackgroundColor = (ConsoleColor)cc.getColor(bg);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else if (cmd.ToString().StartsWith("dog"))
                {
                    var dogGarnFile = cmd.ToString().Remove(0, 4);
                    Console.WriteLine(readFile(dogGarnFile));
                }
                else if (cmd.ToString().StartsWith("setMacAddress"))
                {
                    byte[] macBuffer = new byte[1];
                    Cosmos.Common.Extensions.ByteConverter.SetUInt32(macBuffer, 0, 100);
                    int offset = 0;
                    MACAddress mac = new MACAddress(macBuffer, offset);
                }
                else if (cmd.ToString().StartsWith("setIPAddress"))
                {
                    byte firstByte = Convert.ToByte(int.Parse(args[1]));
                    byte secondByte = Convert.ToByte(int.Parse(args[2]));
                    byte thirdByte = Convert.ToByte(int.Parse(args[3]));
                    byte fourthByte = Convert.ToByte(int.Parse(args[4]));
                    Address addr = new Address(firstByte, secondByte, thirdByte, fourthByte);
                }
                else if (cmd.ToString().StartsWith("samlang"))
                {
                    var samlangFile = cmd.ToString().Remove(0, 8);
                    string samlangFileContents = "";
                    try
                    {
                        var hello_file = fs.GetFile(current_directory + samlangFile);
                        var hello_file_stream = hello_file.GetFileStream();

                        if (hello_file_stream.CanRead)
                        {
                            byte[] text_to_read = new byte[hello_file_stream.Length];
                            hello_file_stream.Read(text_to_read, 0, (int)hello_file_stream.Length);
                            samlangFileContents = Encoding.Default.GetString(text_to_read);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }

                    foreach (var line in samlangFileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        try
                        {
                            SamLangParser.parseSamLang(line);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
                else if (cmd == "math")
                {
                    int ans = 0;
                    Console.WriteLine("(M)ultiply");
                    Console.WriteLine("(A)dd");
                    Console.WriteLine("(S)ubtract");
                    Console.WriteLine("(D)ivide");
                    Console.Write("MATH> ");
                    string mathOperation = Console.ReadLine().ToLower();
                    Console.Write("First number> ");
                    string FirstNumMath = Console.ReadLine();
                    Console.Write("Second number> ");
                    string SecondNumMath = Console.ReadLine();
                    int first = Convert.ToInt32(FirstNumMath);
                    int second = Convert.ToInt32(SecondNumMath);
                    if (mathOperation == "m")
                    {
                        ans = first * second;
                        Console.WriteLine(ans.ToString());
                    }
                    else if (mathOperation == "a")
                    {
                        ans = first + second;
                        Console.WriteLine(ans.ToString());
                    }
                    else if (mathOperation == "s")
                    {
                        ans = first - second;
                        Console.WriteLine(ans.ToString());
                    }
                    else if (mathOperation == "d")
                    {
                        ans = first / second;
                        Console.WriteLine(ans.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Unknown operation!");
                    }
                }
                else if (cmd.StartsWith("cmds") || cmd.StartsWith("help"))
                {
                    try
                    {
                        if (args[1] == "1")
                        {
                            Console.WriteLine("DoorsOS - Commands");
                            Console.WriteLine("---");
                            Console.WriteLine("about - about this copy of DoorsOS.");
                            Console.WriteLine("echo - echos back value you pass to it");
                            Console.WriteLine("2file - writes a specified value to a file.");
                            Console.WriteLine("mkfile - creates a file at specified directory");
                            Console.WriteLine("mkdir - creates the specified directory");
                            Console.WriteLine("del - deletes a file or directory.");
                            Console.WriteLine("cd - changes directory to passed directory");
                            Console.WriteLine("***MORE ON PAGE #2***");
                        }
                        else if (args[1] == "2")
                        {
                            Console.WriteLine("dir - list contents of directory");
                            Console.WriteLine("clear - clears screen");
                            Console.WriteLine("cmds/help - this.");
                            Console.WriteLine("dog - get contents of file.");
                            Console.WriteLine("samlang - run a file as samlang");
                            Console.WriteLine("math - calculates a math operation with 2 numbers.");
                            Console.WriteLine("getMACAddr - gets your MAC address.");
                            Console.WriteLine("netAvailable - check to see if networking is available.");
                            Console.WriteLine("***MORE ON PAGE #3***");
                        }
                        else if (args[1] == "3")
                        {
                            Console.WriteLine("shutdown - shuts down your pc.");
                            Console.WriteLine("reboot - reboots your pc.");
                            Console.WriteLine("date - gets date in (M)M/DD/YY format.");
                            Console.WriteLine("time - gets time in 24 hour format.");
                            //Console.WriteLine("color [FG] [BG] - sets foreground/background color of console.");
                            //Console.WriteLine("color help - lists colors.");
                            Console.WriteLine("micro - MIV alt.");
                            Console.WriteLine("setMACAddr - sets your MAC address.");
                            Console.WriteLine("setIPAddr - sets your IP address, example: setIPAddr 192 168 0 42");
                            Console.WriteLine("***MORE ON PAGE #4***");


                        }
                        else if (args[1].ToLower() == "4")
                        {
                            Console.WriteLine("sOracle - talks to you");
                            Console.WriteLine("execWdcl - executes DOS binaries (Buggy WIP! Only partial 1.x support!)");
                            Console.WriteLine("execBin - executes .bin binaries (WIP!)");
                            Console.WriteLine("drives - list disks"); //similar to the old listvol
                        }
                        else if (args[1].ToLower() == "5")
                        {
                            Console.WriteLine("Invalid page! Try 'cmds 1'");
                        }
                        else
                        {
                            Console.WriteLine("Invalid page! Try 'cmds 1'");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No page! Try 'cmds 1'");
                    }
                }
                else
                {
                    Console.WriteLine("Command not found. (Pro tip: 'cmds 1' for commands!)");
                }
            }
        }
        private string[] GetDirFadr(string adr) // Get Directories From Address
        {
            var dirs = Directory.GetDirectories(adr);
            return dirs;
        }

    }
}