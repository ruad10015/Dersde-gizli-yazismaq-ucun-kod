using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp21
{
    public class Program
    {
            public static int RemotePort { get; set; }
            public static int LocalPort { get; set; }
            public static IPAddress RemoteIPAddress { get; set; }
        static void Main(string[] args)
        {
            Console.SetWindowSize(80, 40);
            Console.Title = "Chat APP";
            
            Console.WriteLine("Enter IP Adress: ");
            RemoteIPAddress=IPAddress.Parse(Console.ReadLine());
            
            Console.WriteLine("Enter Remote Port: ");
            RemotePort=int.Parse(Console.ReadLine());
            
            Console.WriteLine("Enter Local Port: ");
            LocalPort=int.Parse(Console.ReadLine());

            Task.Factory.StartNew(() =>
            {
                Listener();
            },TaskCreationOptions.LongRunning);

            Console.ForegroundColor = ConsoleColor.Red;

            while (true)
            {
                Client(Console.ReadLine());
            }

        }

        private static void Client(string data)
        {
            using(var client=new UdpClient())
            {
                try
                {
                    var ep = new IPEndPoint(RemoteIPAddress, RemotePort);
                    var bytes=Encoding.UTF8.GetBytes(data);
                    client.Send(bytes, bytes.Length, ep);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void Listener()
        {
            try
            {
                while (true)
                {
                    UdpClient uclient= new UdpClient(LocalPort);
                    IPEndPoint ep=new IPEndPoint(RemoteIPAddress,RemotePort);
                    var response = uclient.Receive(ref ep);

                    var data = Encoding.UTF8.GetString(response);

                    Console.ForegroundColor= ConsoleColor.Green;
                    Console.WriteLine($"{ep.Address} - data : {data}");
                    Console.ForegroundColor= ConsoleColor.Red;

                    uclient.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
