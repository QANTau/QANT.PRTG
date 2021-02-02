using System;
using CLAP;


namespace QANT.PRTG
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            {
#if DEBUG
                    System.Console.WriteLine("QANT PRTG Sensors");
                    System.Console.WriteLine();
                    System.Console.Write("Arguments: ");
                    var newArgs = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newArgs))
                        args = newArgs.Split(' ');
#endif
                try
                {
                        Parser.Run<Sensor>(args);
                }
                catch (Exception ex)
                {
                    Error.WriteOutput("Application Exception (" + ex.Message + ")");
                }
#if DEBUG
                    System.Console.ReadLine();
#endif
            }
        }
    }
}
