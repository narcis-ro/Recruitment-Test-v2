using System.Threading;
using System.Threading.Tasks;
using JG.Infrastructure.AspNetCore.StartupTasks;
using LiteDB;

namespace JG.FinTechTest
{
    /// <inheritdoc />
    public class LiteDbStartupTask : IStartupTask
    {
        private readonly LiteDatabase _db;

        /// <inheritdoc />
        public LiteDbStartupTask(LiteDatabase db)
        {
            _db = db;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            // Note: Force database creation at startup
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task ShutdownAsync(CancellationToken cancellationToken = default)
        {
            // Note: Make sure the database is properly closed before shutting down
            _db.Dispose();

            return Task.CompletedTask;
        }
    }
}