using KeyboardInputsAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rmopo
{
    internal class Program
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        static string pathpaste = "", pathcopy = "";
        static Dictionary<string, string> filespaste = new Dictionary<string, string>(), filescopy = new Dictionary<string, string>();
        public static uint getForegroundProcessPid()
        {
            uint processID = 0;
            IntPtr hWnd = GetForegroundWindow();
            GetWindowThreadProcessId(hWnd, out processID);
            return processID;
        }
        public static void OnKeyDown()
        {
            KeyboardInput ki = new KeyboardInput();
            ki.Scan();
            ki.BeginPolling();
            while (true)
            {
                if (ki.KeyboardKeyF1 & getForegroundProcessPid() == Process.GetCurrentProcess().Id)
                {
                    const string message = "• Author: Michaël André Franiatte.\n\r\n\r• Contact: michael.franiatte@gmail.com.\n\r\n\r• Publisher: https://github.com/michaelandrefraniatte.\n\r\n\r• Copyrights: All rights reserved, no permissions granted.\n\r\n\r• License: Not open source, not free of charge to use.";
                    const string caption = "About";
                    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                System.Threading.Thread.Sleep(60);
            }
        }
        static void Main(string[] args)
        {
            Task.Run(() => { OnKeyDown(); });
            Console.WriteLine("Actual folder path here for paste:");
            Console.WriteLine(pathpaste = System.Windows.Forms.Application.StartupPath);
            Console.WriteLine("Enter a folder path to copy here:");
            pathcopy = Console.ReadLine();
            string[] fileNamesToCopy = Directory.GetFiles(pathcopy);
            foreach (string fileName in fileNamesToCopy)
            {
                filescopy.Add(fileName.Replace(pathcopy, ""), System.IO.File.GetLastWriteTime(fileName).ToString("dd/MM/yy HH:mm:ss"));
            }
            string[] dirsToCopy = Directory.GetDirectories(pathcopy, "*", SearchOption.AllDirectories);
            foreach (string dir in dirsToCopy)
            {
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    filescopy.Add(file.Replace(pathcopy, ""), System.IO.File.GetLastWriteTime(file).ToString("dd/MM/yy HH:mm:ss"));
                }
            }
            string[] fileNamesToPaste = Directory.GetFiles(pathpaste);
            foreach (string fileName in fileNamesToPaste)
            {
                filespaste.Add(fileName.Replace(pathpaste, ""), System.IO.File.GetLastWriteTime(fileName).ToString("dd/MM/yy HH:mm:ss"));
            }
            string[] dirsToPaste = Directory.GetDirectories(pathpaste, "*", SearchOption.AllDirectories);
            foreach (string dir in dirsToPaste)
            {
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    filespaste.Add(file.Replace(pathpaste, ""), System.IO.File.GetLastWriteTime(file).ToString("dd/MM/yy HH:mm:ss"));
                }
            }
            foreach (KeyValuePair<string, string> entry in filescopy)
            {
                string value;
                if (filespaste.TryGetValue(entry.Key, out value))
                {
                    if (entry.Value != value) 
                    {
                        File.Copy(pathcopy + "/" + entry.Key, pathpaste + "/" + entry.Key, true);
                    }
                }
                else
                {
                    string dirpath = Path.GetDirectoryName(pathpaste + "/" + entry.Key);
                    if (!Directory.Exists(dirpath)) 
                        Directory.CreateDirectory(dirpath);
                    File.Copy(pathcopy + "/" + entry.Key, pathpaste + "/" + entry.Key);
                }
            }
            Console.WriteLine("done");
            Console.Read();
        }
    }
}