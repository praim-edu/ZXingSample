using SmbLibraryStd;
using SmbLibraryStd.Authentication.GSSAPI;
using SmbLibraryStd.Authentication.NTLM;
using SmbLibraryStd.Server;
using SmbLibraryStd.Win32;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ZXingSample
{
    class XASmbServer
    {
        //See https://github.com/j4m3z0r/SmbLibraryStd/blob/master/SMBServer/ServerUI.cs

        SMBServer server;

        public string IpAddress { get; }
        public string UserName { get; }
        public string UserPassword { get; }

        public XASmbServer(string ipAddress, string userName, string userPassword)
        {
            IpAddress = ipAddress;
            UserName = userName;
            UserPassword = userPassword;
        }


        public void Start()
        {
            IPAddress serverAddress = IPAddress.Parse(IpAddress);
            SMBTransportType transportType = SMBTransportType.DirectTCPTransport;

            UserCollection users = new UserCollection();
            users.Add(UserName, UserPassword);

            NTLMAuthenticationProviderBase authenticationMechanism = new IndependentNTLMAuthenticationProvider(users.GetUserPassword);

            SMBShareCollection shares = new SMBShareCollection();
            FileSystemShare share = new FileSystemShare("documents", new NTDirectoryFileSystem("/storage/emulated/0/Documents"));
            share.AccessRequested += delegate (object sender, AccessRequestArgs args)
            {
                // allow read and write on share
                args.Allow = true;
            };
            shares.Add(share);

            GSSProvider securityProvider = new GSSProvider(authenticationMechanism);
            server = new SmbServer2(shares, securityProvider);

            try
            {
                server.Start(serverAddress, transportType, true, true);
            }
            catch (Exception ex)
            {

            }
        }

        public void Stop()
        {
            if (server != null)
                server.Stop();
        }
    }

    // create class SmbServer2 to override SMB default port 445 to 4445
    public class SmbServer2 : SMBServer
    {
        public override int DirectTCPPort => 4445;

        public SmbServer2(SMBShareCollection shares, GSSProvider securityProvider) : base(shares, securityProvider)
        {
        }
    }

    public class UserCollection : List<User>
    {
        public void Add(string accountName, string password)
        {
            Add(new User(accountName, password));
        }

        public int IndexOf(string accountName)
        {
            for (int index = 0; index < this.Count; index++)
            {
                if (string.Equals(this[index].AccountName, accountName, StringComparison.OrdinalIgnoreCase))
                {
                    return index;
                }
            }
            return -1;
        }

        public string GetUserPassword(string accountName)
        {
            int index = IndexOf(accountName);
            if (index >= 0)
            {
                return this[index].Password;
            }
            return null;
        }

        public List<string> ListUsers()
        {
            List<string> result = new List<string>();
            foreach (User user in this)
            {
                result.Add(user.AccountName);
            }
            return result;
        }
    }

    public class User
    {
        public string AccountName;
        public string Password;

        public User(string accountName, string password)
        {
            AccountName = accountName;
            Password = password;
        }
    }
}
