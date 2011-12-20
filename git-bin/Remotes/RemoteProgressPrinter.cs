using System;
using System.Collections.Generic;
using System.Linq;

namespace GitBin.Remotes
{
    public class RemoteProgressPrinter : IDisposable
    {
        private readonly IRemote _remote;
        private readonly List<int> _percentagesToReport;
        private bool _hasPrintedAnything;

        public RemoteProgressPrinter(int chunkNumber, int totalChunks, IRemote remote)
        {
            GitBinConsole.WriteNoPrefix("  [{0}/{1}] -> ", chunkNumber, totalChunks);

            _remote = remote;
            _remote.ProgressChanged += OnProgressChanged;

            _percentagesToReport = new List<int>{0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100};
        }

        public void Dispose()
        {
            GitBinConsole.WriteNoPrefix(Environment.NewLine);

            _remote.ProgressChanged -= OnProgressChanged;
        }

        private void OnProgressChanged(int percentageComplete)
        {
            var percentagesToPrint = GetPercentagesBelowOrEqual(percentageComplete);

            foreach (var percentToPrint in percentagesToPrint)
            {
                GitBinConsole.WriteNoPrefix("{0}{1}", _hasPrintedAnything ? ".." : string.Empty, percentToPrint);
                _hasPrintedAnything = true;
                _percentagesToReport.Remove(percentToPrint);
            }

            GitBinConsole.Flush();
        }

        private List<int> GetPercentagesBelowOrEqual(int percent)
        {
            return _percentagesToReport
                .TakeWhile(x => x <= percent)
                .ToList();
        }
    }
}