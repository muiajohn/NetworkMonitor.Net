using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetFwTypeLib;
using System.Collections;

namespace NetworkMonitor.Windows
{
    public class NetFirewall
    {
        private const string CLSID_FIREWALL_MANAGER = "{304CE942-6E39-40D8-943A-B913C40C9CD4}";
        private const string PROGID_AUTHORIZED_APPLICATION = "HNetCfg.FwAuthorizedApplication";
        private const string PROGID_OPEN_PORT = "HNetCfg.FWOpenPort";

        private INetFwMgr m_NetFwMgr;

        public NetFirewall()
        {
            m_NetFwMgr = GetFirewallManager();
        }

        private INetFwMgr GetFirewallManager()
        {
            Type objectType = Type.GetTypeFromCLSID(
                  new Guid(CLSID_FIREWALL_MANAGER));
            return Activator.CreateInstance(objectType)
                  as INetFwMgr;
        }

        public bool IsFirewallEnabled
        {
            get
            {
                bool isFirewallEnabled =
                    m_NetFwMgr.LocalPolicy.CurrentProfile.FirewallEnabled;

                return isFirewallEnabled;
            }
        }

        public void EnableFirewall()
        {
            if (!IsFirewallEnabled)
            {
                m_NetFwMgr.LocalPolicy.CurrentProfile.FirewallEnabled = true;
            }
        }

        public void DisableFirewall()
        {
            if (IsFirewallEnabled)
            {
                m_NetFwMgr.LocalPolicy.CurrentProfile.FirewallEnabled = false;
            }
        }

        public bool AuthorizeApplicationDefault(string title, string applicationPath)
        {
            // Create the type from prog id
            Type type = Type.GetTypeFromProgID(PROGID_AUTHORIZED_APPLICATION);
            INetFwAuthorizedApplication auth = Activator.CreateInstance(type)
                as INetFwAuthorizedApplication;
            auth.Name = title;
            auth.ProcessImageFileName = applicationPath;
            auth.Enabled = true;

            try
            {
                m_NetFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(auth);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public bool AuthorizeApplication(string title, string applicationPath)
        {
            return AuthorizeApplication(title, applicationPath,
                NET_FW_SCOPE_.NET_FW_SCOPE_ALL,
                NET_FW_IP_VERSION_.NET_FW_IP_VERSION_ANY);
        }

        public bool AuthorizeApplication(string title, string applicationPath,
            NET_FW_SCOPE_ scope, NET_FW_IP_VERSION_ ipVersion)
        {
            // Create the type from prog id
            Type type = Type.GetTypeFromProgID(PROGID_AUTHORIZED_APPLICATION);
            INetFwAuthorizedApplication auth = Activator.CreateInstance(type)
                as INetFwAuthorizedApplication;
            auth.Name = title;
            auth.ProcessImageFileName = applicationPath;
            auth.Scope = scope;
            auth.IpVersion = ipVersion;
            auth.Enabled = true;

            //INetFwMgr manager = GetFirewallManager();
            try
            {
                m_NetFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(auth);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public bool GloballyOpenPort(string title, int portNo,
            NET_FW_SCOPE_ scope, NET_FW_IP_PROTOCOL_ protocol,
            NET_FW_IP_VERSION_ ipVersion)
        {
            Type type = Type.GetTypeFromProgID(PROGID_OPEN_PORT);
            INetFwOpenPort port = Activator.CreateInstance(type)
                as INetFwOpenPort;
            port.Name = title;
            port.Port = portNo;
            port.Scope = scope;
            port.Protocol = protocol;
            port.IpVersion = ipVersion;

            //INetFwMgr manager = GetFirewallManagerCached();
            try
            {
                m_NetFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(port);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public bool IsInAuthorizeApplications(string title, string applicationPath)
        {
            IEnumerator enumer = m_NetFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.GetEnumerator();

            while (enumer.MoveNext())
            {
                INetFwAuthorizedApplication app = enumer.Current as INetFwAuthorizedApplication;

                if (app.Name == title && app.ProcessImageFileName == applicationPath)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
