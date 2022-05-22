using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace monitorServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("键入本地IP地址");
            IPAddress ip = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("键入监听端口");
            int port = int.Parse(Console.ReadLine());
            TcpListener myList = new TcpListener(ip, port);
            myList.Start();
            Console.WriteLine("开启监听:" + myList.LocalEndpoint);

            Socket SocketClip = myList.AcceptSocket();
            Console.WriteLine("连接来自 " + SocketClip.RemoteEndPoint);

            while (SocketClip.Connected)
            {
                try
                {
                    ASCIIEncoding asen = new ASCIIEncoding();
                    SocketClip.Send(asen.GetBytes(Console.ReadLine()));
                    byte[] Log_byte = new byte[999999999];
                    int k = SocketClip.Receive(Log_byte);
                    string Log = "";
                    for (int i = 0; i < k; i++)
                    {
                        Log += Convert.ToChar(Log_byte[i]);
                    }
                    if (k > 2000)
                    {
                        string time = DateTime.Now.ToString("hh时mm分ss秒"); ;
                        Console.WriteLine("正在保存图像");
                        MemoryStream mStream = new MemoryStream();
                        mStream.Write(Log_byte, 0, Log_byte.Length);
                        Image Jpge = Bitmap.FromStream(mStream, true);
                        Jpge.Save(time + ".jpg", ImageFormat.Jpeg);
                        Console.WriteLine("保存图像完成");
                    }
                    else
                    {
                        Console.WriteLine(Log);
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("远程主机强迫关闭了连接");
                    SocketClip = myList.AcceptSocket();
                    Console.WriteLine("连接来自 " + SocketClip.RemoteEndPoint);
                }

            }
            Console.ReadKey();
        }
    }
}
