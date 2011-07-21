using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace NetworkMonitor.Common
{
    public class IPNotifyItem
    {
        public PackageDirect Director;
        public IPAddress SourceIP;
        public IPAddress DestIP;
        public Protocol ProtocolType;
        public long Length;
        public long MessageLength;
        public string SourcePort;
        public string DestPort;

        public string FLAG
        {
            get
            {
                return string.Format("{0}_{1}_(2)", Director, SourceIP, DestIP);
            }
        }
    }
}
