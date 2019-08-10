using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AlbumViewerNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();

            var hostUrl = configuration["hosturl"];
            if (string.IsNullOrEmpty(hostUrl))
                hostUrl = "http://192.168.0.107:61152";

            var host = new WebHostBuilder().UseKestrel().UseUrls(hostUrl).UseContentRoot(Directory.GetCurrentDirectory()).UseIISIntegration().UseStartup<Startup>().Build();
            host.Run();
        }
    }
}
