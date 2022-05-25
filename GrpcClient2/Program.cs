using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcServer;
using System;
using System.Net.Http;

namespace GrpcClient2
{
    class Program
    {
        static void Main(string[] args)
        {
            // The port number must match the port of the gRPC server.
            //using var channel = GrpcChannel.ForAddress("http://localhost:5000");

            var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
            {
                HttpHandler = new GrpcWebHandler(new HttpClientHandler())
            });

            var client = new Greeter.GreeterClient(channel);
            var reply = client.SayHello(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.ToString());
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
