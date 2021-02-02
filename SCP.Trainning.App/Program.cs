using Domain.Abstraction.Service;
using Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace SCP.Trainning.App
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            int result;

            try
            {
                var decisions = ValidateArgs(args);

                var service = GetProcessService();
                await service.Init(decisions);

                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat("Finish with error - ", ex));
                result = 1;
            }

            return result;
        }

        private static ProcessServiceDecisions ValidateArgs(string[] args)
        {
            ProcessServiceDecisions result;

            if (args.Length == 0)
            {
                result = new ProcessServiceDecisions();
            }
            else if (args.Length == 6)
            {
                bool groupHistory = bool.Parse(args[0]);
                bool normalization = bool.Parse(args[1]);
                bool rectification = bool.Parse(args[2]);
                bool similarLocations = bool.Parse(args[3]);
                bool mergeDatabaseAndUpdateSystemInfo = bool.Parse(args[4]);
                int maxDegreesOfParallelism = int.Parse(args[5]);

                result = new ProcessServiceDecisions(groupHistory, normalization, rectification, similarLocations, mergeDatabaseAndUpdateSystemInfo, maxDegreesOfParallelism);
            }
            else
            {
                throw new Exception("Invalid parameters");
            }

            return result;
        }

        private static IProcessService GetProcessService()
        {
            var startup = new Startup();
            startup.Configure("SCP.Normalization.App");
            var provider = startup.BuildServiceProvider();

            return provider.GetService<IProcessService>();
        }
    }
}
