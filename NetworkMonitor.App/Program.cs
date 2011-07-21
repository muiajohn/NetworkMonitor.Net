using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkMonitor.Common;
using System.Net.Sockets;
using NetworkMonitor.Windows;
using System.IO;

namespace NetworkMonitor.App
{
    class Program
    {
        static void Main(string[] args)
        {
            NetFirewall nfw = new NetFirewall();

            ColorConsole cc = new ColorConsole();

            try
            {
                if (nfw.IsFirewallEnabled)
                {
                    nfw.AuthorizeApplication("NetworkMonitor", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetworkMonitor.App.exe"));
                }
                else
                {
                    cc.TextColor(Color.Blue);
                    Console.WriteLine("未检测到Windows防火墙在运行。如果安装有其它的防火墙，请将该程序加入例外列表中");
                }

                cc.TextColor(Color.Green);
                Console.WriteLine("输入要监控的IP地址:");

                string bindingip = Console.ReadLine();

                string[] bindingAddress;

                if (bindingip.Contains(":"))
                {
                    bindingAddress = bindingip.Split(new char[] { ':' });
                }
                else
                {
                    bindingAddress = new string[] { bindingip };
                }

                NetProvider m_provider;
                RawSocket m_socket;

                m_provider = new NetProvider(bindingAddress[0]);

                if (1 == bindingAddress.Length)
                {
                    m_socket = new RawSocket(AddressFamily.InterNetwork, m_provider);
                }
                else
                {
                    m_socket = new RawSocket(AddressFamily.InterNetwork, m_provider, Convert.ToInt32(bindingAddress[1]));
                }

                m_socket.Start(bindingAddress[0]);
            }
            catch (Exception eX)
            {
                Console.WriteLine(eX.Message);
            }

            Console.ReadLine();
        }
    }
}
