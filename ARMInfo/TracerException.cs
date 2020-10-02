using System;
using System.IO;
using System.Windows;

namespace ARMInfo
{

    internal interface ITracerException
    {
        void Append(string err);
    }
    internal class TracerException : ITracerException
    {
        static string TraceFile = "Trace.txt";

        private static void LogError(string msg)
        {
            if (File.Exists("Trace.txt"))
            {
                using (var file = File.AppendText(TraceFile))
                {
                    file.WriteLine(msg);
                    file.Close();
                }
            }
        }

        public TracerException()
        {
        }

        public void Append(string err)
        {
            LogError(err);
        }
    }
}