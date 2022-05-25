using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GrpcServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(kestrelOptions =>
                    {
                        kestrelOptions.ConfigureHttpsDefaults(https =>
                        {
                            https.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                            https.ServerCertificate = new X509Certificate2("domain.pfx", "grpc");
                        });

                        kestrelOptions.Listen(IPAddress.Parse("127.0.0.1"), 5001, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;


                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
