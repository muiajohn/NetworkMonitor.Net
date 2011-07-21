using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkMonitor.Common;
using System.Net.Sockets;

namespace NetworkMonitor.App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
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
