using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rmopo
{
    internal class Program
    {
        static string pathpaste = "", pathcopy = "";
        static Dictionary<string, string> filespaste = new Dictionary<string, string>(), filescopy = new Dictionary<string, string>();
        static void Main(string[] args)
        {
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