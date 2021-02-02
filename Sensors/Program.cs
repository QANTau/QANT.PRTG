using System;
using CLAP;


namespace QANT.PRTG
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            {
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
