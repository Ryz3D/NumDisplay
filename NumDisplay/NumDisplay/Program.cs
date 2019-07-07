using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace NumDisplay
{
    class Program
    {
        private static readonly string PATH_DISPLAYS = "C:/Program Files (x86)/Steam/steamapps/common/Homebrew - Vehicle Sandbox/hb146_Data/";
        private static readonly string PATH_SWITCHES = "C:/Program Files (x86)/Steam/steamapps/common/Homebrew - Vehicle Sandbox/hb146_Data/Switch.txt";

        static void Main(string[] args)
        {
            IPAddress ip = GetLocalAddress();
            if (ip == null)
                return;

            Console.WriteLine("Address: " + ip.ToString());
            
            TcpListener server = new TcpListener(ip, 6000);
            server.Start();

            new Thread(() =>
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();

                    StreamReader reader = new StreamReader(client.GetStream());
                    StreamWriter writer = new StreamWriter(client.GetStream());

                    Console.WriteLine("New Client: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

                    string switchCount = File.ReadLines(PATH_SWITCHES).ToArray()[0];
                    writer.WriteLine(switchCount);
                    writer.Flush();
                    Console.WriteLine("There appear to be " + switchCount + "switches");

                    new Thread(() =>
                    {
                        try
                        {
                            while (true)
                            {
                                string data = reader.ReadLine();

                                try
                                {
                                    int switchIndex = int.Parse(data);
                                    File.WriteAllText(PATH_SWITCHES, string.Format("{0}i1\r\n", switchIndex));
                                    Thread.Sleep(500);
                                    File.WriteAllText(PATH_SWITCHES, string.Format("{0}i0\r\n", switchIndex));
                                } catch (FormatException)
                                {
                                    Console.WriteLine("Eh I'm receiving weird stuff...");
                                }
                            }
                        } catch (Exception)
                        {
                            Console.WriteLine("Hmm... i think there is no Connection... :(");
                        }
                    }).Start();

                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Path = PATH_DISPLAYS;
                    watcher.Changed += (sender, e) =>
                    {
                        if (e.Name == "NumDisplay.txt" && e.ChangeType == WatcherChangeTypes.Changed)
                        {
                            try
                            {
                                string data = File.ReadAllText(PATH_DISPLAYS + e.Name);
                                Console.Write("New Change: " + data);
                                writer.Write(data);
                                writer.Flush();
                                Console.WriteLine("!");
                            }
                            catch (IOException)
                            {
                                Console.WriteLine("I don't have access :(");
                            }
                        }
                    };
                    watcher.EnableRaisingEvents = true;
                    Console.ReadLine();
                } catch (SocketException)
                {
                    Console.WriteLine("Connection lost D:");
                }
            }).Start();
        }

        private static IPAddress GetLocalAddress()
        {
            IPAddress[] addr = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(s => s.MapToIPv4() == s).ToArray();
            if (addr.Count() == 1)
                return addr[0];

            for (int i = 0; i < addr.Count(); i++)
                Console.WriteLine("{0}: {1}", i, addr[i].ToString());

            Console.Write("Choose IP Address > ");
            string index = Console.ReadLine();
            return addr[int.Parse(index)];
        }
    }
}