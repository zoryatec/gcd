using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services.RemoteFileSystem;
// Helper class to handle SMB connection
public class NetworkConnection : IDisposable
{
    private readonly string _networkName;

    public NetworkConnection(string networkName, NetworkCredential credentials)
    {
        _networkName = networkName;

        var netResource = new NETRESOURCE
        {
            Scope = 0,
            Type = 1,
            DisplayType = 0,
            Usage = 0,
            LocalName = null,
            RemoteName = networkName,
            Provider = null
        };

        var result = WNetAddConnection2(netResource, credentials.Password, credentials.UserName, 0);

        if (result != 0)
        {
            throw new IOException($"Error connecting to remote share. Error code: {result}");
        }
    }

    public void Dispose()
    {
        WNetCancelConnection2(_networkName, 0, true);
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public class NETRESOURCE
    {
        public int Scope;
        public int Type;
        public int DisplayType;
        public int Usage;
        public  string LocalName = null;
        public  string RemoteName = null;
        public  string Comment = null;
        public  string Provider = null;
    }

    [System.Runtime.InteropServices.DllImport("mpr.dll")]
    private static extern int WNetAddConnection2(NETRESOURCE netResource, string password, string username, int flags);

    [System.Runtime.InteropServices.DllImport("mpr.dll")]
    private static extern int WNetCancelConnection2(string name, int flags, bool force);
}