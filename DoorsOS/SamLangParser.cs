using System;
using System.Collections.Generic;
using System.Text;

namespace DoorsOS
{
    class SamLangParser
    {
        public static string nextVar = "";
        public static bool skipBlankening = true;
        public static int nextVarPos = -1;
        public static string parseSamLang(string line)
        {
            var samlangArgs = line.Split(' ');
            try
            {
                samlangArgs[nextVarPos] = nextVar;
            }
            catch
            {
                //do nothing.
            }
            var vars = new Dictionary<string, object>();
            if (line.ToString().StartsWith("print-nlb"))
            {
                var printingNLB = line.ToString().Remove(0, 11);
                Console.Write(printingNLB);
                return printingNLB;
            }
            else if (line.ToString().StartsWith("print"))
            {
                var printing = line.ToString().Remove(0, 6);
                Console.WriteLine(printing);
                return printing + "\n";
            }
            else if (line.ToString().StartsWith("str"))
            {
                var printing = line.ToString().Remove(0, 4);
                return printing;
            }
            else if (line.ToString().StartsWith("beep"))
            {
                try
                {
                    console.beep(int.Parse(samlangArgs[1]));
                    return "*BEEEEP*";
                }
                catch
                {
                    console.beep(0);
                    return "*BEEP*";
                }
            }
            else if (line.ToString().StartsWith("math+"))
            {
                var result = int.Parse(samlangArgs[1]) + int.Parse(samlangArgs[2]);
                Console.WriteLine(result);
                return result.ToString();
            }
            else if (line.ToString().StartsWith("math/"))
            {
                var result = int.Parse(samlangArgs[1]) / int.Parse(samlangArgs[2]);
                Console.WriteLine(result);
                return result.ToString();
            }
            else if (line.ToString().StartsWith("math*"))
            {
                var result = int.Parse(samlangArgs[1]) * int.Parse(samlangArgs[2]);
                Console.WriteLine(result);
                return result.ToString();
            }
            else if (line.ToString().StartsWith("math-"))
            {
                var result = int.Parse(samlangArgs[1]) / int.Parse(samlangArgs[2]);
                Console.WriteLine(result);
                return result.ToString();
            }
            else if (line.ToString().StartsWith("push_var"))
            {
                var varbl = line.Replace("push_var " + samlangArgs[1] + " ", "").ToString();
                vars.Add(samlangArgs[1], parseSamLang(varbl));
                return "Set variable!";
            }
            else if (line.ToString().StartsWith("set_next_arg_from_var"))
            {
                var varbl = vars[samlangArgs[1]];
                nextVar = varbl.ToString();
                nextVarPos = int.Parse(samlangArgs[2]);
                skipBlankening = true;
                return "next arg set";
            }
            else
            {
                Console.WriteLine("!!! ERROR - COMMAND NOT FOUND!!!");
                return "E: CMD NOT FOUND!";
            }
            if (skipBlankening == false)
            {
                nextVar = "";
                nextVarPos = -1;
                return "";
            }
        }
    }
}