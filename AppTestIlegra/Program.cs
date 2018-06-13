using ServiceImportFiles.Service;
using System;
using System.Threading.Tasks;

namespace AppTestIlegra
{
    class Program
    {
        static void Main(string[] args)
        {
            FileService service = new FileService($"{Environment.GetEnvironmentVariable("USERPROFILE")}\\data");
            Task.Run(() => service.Start());
            Console.Read();
        }
    }
}
