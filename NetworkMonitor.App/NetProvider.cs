using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkMonitor.Common;
using NetworkMonitor.Windows;

namespace NetworkMonitor.App
{
    public class NetProvider : SocketProvider
    {
        private ColorConsole m_ColorConsole;

        public NetProvider(string bindingip)
            : base(bindingip)
        {
            m_ColorConsole = new ColorConsole();
        }

        protected override void OnIPNotify(IPNotifyItem stat)
        {
            m_ColorConsole.TextColor(Color.Red);
            if (PackageDirect.IN == stat.Director)
            {
                Console.WriteLine(
                    "接受数据\t源地址:{0}\t\t目的地址:{1}\t\t包大小:{2}",
                    stat.SourceIP, stat.DestIP, stat.Length);
            }
            else
            {
                m_ColorConsole.TextColor(Color.Green);
                Console.WriteLine(
                    "发送数据\t源地址:{0}\t\t目的地址:{1}\t\t包大小:{2}",
                    stat.SourceIP, stat.DestIP, stat.Length);
            }
        }
    }
}
