using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol;

namespace OmniSharp.Extensions.LanguageServer.Server.Handlers
{
    public class ShutdownHandler : IShutdownHandler
    {
        public event ShutdownEventHandler Shutdown;

        public bool ShutdownRequested { get; private set; }

        private readonly TaskCompletionSource<bool> _shutdownSource = new TaskCompletionSource<bool>(TaskContinuationOptions.LongRunning);
        public Task WasShutDown => _shutdownSource.Task;
        public async Task Handle(object request, CancellationToken token)
        {
            await Task.Yield(); // Ensure shutdown handler runs asynchronously.

            ShutdownRequested = true;
            try
            {
                Shutdown?.Invoke(ShutdownRequested);
            }
            finally
            {
                _shutdownSource.SetResult(true); // after all event sinks were notified
            }
        }

    }
}
