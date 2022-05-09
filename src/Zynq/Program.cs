using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Renci.SshNet;

namespace Zynq;

class Program
{
    class RemoteInfo
    {
        public string User { get; init; }
        public string Uri { get; init; }
        public string Path { get; init; }

        private RemoteInfo(string user, string uri, string path) 
        {
            User = user;
            Uri = uri;
            Path = path;
        }

        public static RemoteInfo Parse(string a)
        {
            return new RemoteInfo(
                user : a.Substring(0, a.IndexOf('@')),
                uri : a.Substring(a.IndexOf('@') + 1, a.LastIndexOf(':') - (a.IndexOf('@') + 1)),
                path : a.Substring(a.LastIndexOf(':') + 1)
            );
        }
    }
    async static Task Main(string[] args)
    {
        if (ParseArgs(args, out string remote, out string source, out string keyPath)) return;

        if (string.IsNullOrEmpty(remote))
        {
            Console.WriteLine("No remote spesified");
            return;
        }

        if (string.IsNullOrEmpty(source) || !Directory.Exists(source))
        {
            Console.WriteLine("Invalid source path");
            return;
        }

        PrivateKeyFile[] keys;
        if (string.IsNullOrEmpty(keyPath))
        {
            var temp = GetKeyFiles();
            if (temp != null)
                keys = temp!;
            else
            {
                Console.WriteLine("Could not automatically find key files");
                return;
            }
        }
        else
        {
            if (File.Exists(keyPath))
                keys = new PrivateKeyFile[] { new PrivateKeyFile(keyPath)};
            else
            {
                Console.WriteLine("Key file does not exist");
                return; 
            }
        }

        RemoteInfo rInfo = RemoteInfo.Parse(remote);

        using SftpClient client = new SftpClient(rInfo.Uri, rInfo.User, keys);

        client.Connect();
        client.ChangeDirectory(rInfo.Path);
    }

    private static PrivateKeyFile[]? GetKeyFiles()
    {
        string? homepath = GetHomePath();

        if (string.IsNullOrEmpty(homepath))
            return null;

        string sshKeyPathBase = Path.Combine(homepath, ".ssh");

        string[] sshDirFiles = Directory.GetFiles(sshKeyPathBase);

        string[] sshSpesialFiles = { "authorized_keys", "known_hosts", "config" };

        var sshKeyFiles =
            from file in sshDirFiles
            where !Path.HasExtension(file) && !sshSpesialFiles.Contains(file)
            select new BufferedStream(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read));

        var pkFile = 
            from file in sshKeyFiles
            select new PrivateKeyFile(file);

        return pkFile.ToArray();
    }

    private static bool ParseArgs(string[] args, out string remote, out string source, out string keyPath)
    {
        string r = string.Empty, s = string.Empty, k = string.Empty;

        bool argError = false;
        foreach (string arg in args)
            try
            {
                if (arg.StartsWith("--remote=") && !string.IsNullOrEmpty(r))
                    r = arg.Substring(arg.IndexOf('=') + 1).Trim(' ', '\'', '\"', '\n', '\t');
                else if (arg.StartsWith("--source=") && !string.IsNullOrEmpty(s))
                    s = arg.Substring(arg.IndexOf('=') + 1).Trim(' ', '\'', '\"', '\n', '\t');
                else if (arg.StartsWith("--key=") && !string.IsNullOrEmpty(k))
                    k = arg.Substring(arg.IndexOf('=') + 1).Trim(' ', '\'', '\"', '\n', '\t');
                else
                {
                    Console.WriteLine("Illegal argument: {0}", arg);
                    argError = true;
                }
            }
            catch (Exception)
            {
                argError = true;
            }

        (remote, source, keyPath) = (r, s, k);

        return argError;
    }

    private static string? GetHomePath()
    {
        if (OperatingSystem.IsWindows())
            return Environment.GetEnvironmentVariable("USERPROFILE");
        else if (OperatingSystem.IsLinux())
            return Environment.GetEnvironmentVariable("HOME");
        else if (OperatingSystem.IsMacOS())
            return Environment.GetEnvironmentVariable("HOME");
        else if (OperatingSystem.IsFreeBSD())
            return Environment.GetEnvironmentVariable("HOME");
        else
            return null;
    }
}
