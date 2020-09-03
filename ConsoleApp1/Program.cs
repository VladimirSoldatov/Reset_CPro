using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Management;

namespace ConsoleApp1
{
    class Program
    {
        static public System.Management.ManagementObjectSearcher Find_Users()
        {
           
            string Q = "SELECT * FROM Win32_Product WHERE Name like '%%КриптоПро CSP%%'";
          
            string result = String.Empty;
            System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(Q);
           
            return searcher;

        }
        static public void Select_User(string name)
        {
            Console.WriteLine(String.Format("Идет поиск установленного {0}...", name));
            string ID = "";
            foreach (ManagementObject User in Find_Users().Get())
            {
               ID = User["IdentifyingNumber"].ToString();
                
            }
            Console.WriteLine(String.Format("Приложение {0} с кодом установки {1} найдено.\nПереустановка начата.", name, ID));
            if (ID != "")
            {
                Process Y = new Process();
                Y.StartInfo.UseShellExecute = false;
                Y.StartInfo.FileName = "msiexec.exe";
                Y.StartInfo.Arguments = " /fa " + ID;
                Y.StartInfo.CreateNoWindow = true;
                Y.Start();
                System.Threading.Thread.Sleep(1000);
                Y.WaitForExit();
                Console.WriteLine(String.Format("Код завершение {0}", Y.ExitCode));

            }


        }
        static void Main(string[] args)
        {
            string message="";
            using (var root = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64))
            {
                try
                {
                    if (System.Environment.Is64BitOperatingSystem)
                    {
                        using (var key2 = root.OpenSubKey(@"Wow6432Node\CLSID", true))
                        {
                            key2.DeleteSubKeyTree("{C8B655BB-28A0-4BB6-BDE1-D0826457B2DF}");
                        }
                    }
                    else
                    {
                        using (var key2 = root.OpenSubKey(@"CLSID", true))
                        {
                            key2.DeleteSubKeyTree("{C8B655BB-28A0-4BB6-BDE1-D0826457B2DF}");
                        }
                    }
                    Console.WriteLine("Сведения о дате устаноки удалены.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    message = e.Message;
                }
                if (message == "")
                    Select_User("КриптоПро CSP");
                else
                    Console.WriteLine("Проверьте права доступа к реестру.");
            };  
        }
    }
}
