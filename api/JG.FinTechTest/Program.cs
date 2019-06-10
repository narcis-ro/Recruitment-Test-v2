using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace JG.FinTechTest.Api
{
    /// <summary>
    ///     FinTech  API
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Main entry-point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the AspNetCore web host.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}
