using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PositionReportPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var positionReportService = new PositionReportService();

            var attempt = 1;

            while (true)
            {
                Console.WriteLine($"Processing Position Reports at {DateTime.Now.ToString()}");

                while (attempt <= 3)
                {
                    try
                    {
                        positionReportService.CollectAndPublishPositionReports().Wait();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Process Failed: {ex.Message}.");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.WriteLine("Waiting 3 seconds before retry.");
                        Task.Delay(3000).Wait();

                        attempt += 1;
                    }
                }

                Console.WriteLine($"Process Completed at {DateTime.Now.ToString()}");

                Task.Delay(2000).Wait();
            }
        }
    }
}
