using System;

namespace DoorsOS
{
    public class console
    {
        internal static void log(string message)
        {
            Console.WriteLine(message);
        }
        internal static void error(string message)
        {
            Console.Error.WriteLine(message);
        }

        internal static void info(string message)
        {
            Console.WriteLine("INFO: " + message);
        }

        internal static void warn(string message)
        {
            Console.Error.WriteLine(message + Environment.NewLine);
        }
        internal static void cls()
        {
            Console.Clear();
        }
        internal static void beep(int time)
        {
            for (int i = 0; i < time - 1; i++)
            {
                Console.WriteLine('\a');
            }
            Console.WriteLine('\a');
        }
        internal static string getInputCoreKernel(string q)
        {
            Console.Write(q);
            return Console.ReadLine();
        }
    }
}
