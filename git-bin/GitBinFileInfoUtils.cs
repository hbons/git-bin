using System;
using System.Collections.Generic;
using System.Linq;

namespace GitBin
{
    public static class GitBinFileInfoUtils
    {
        private static string[] Suffixes = new[] { "B", "k", "M", "G", "T", "P", "E" };

        public static string GetHumanReadableSize(IEnumerable<GitBinFileInfo> fileInfos)
        {
            var totalSize = fileInfos.Sum(fi => fi.Size);

            return GetHumanReadableSize(totalSize);
        }

        public static string GetHumanReadableSize(long numberOfBytes)
        {
            int suffixIndex = 0;
            int increment = 1024;
            double scaledNumberOfBytes = numberOfBytes;

            if (numberOfBytes > 0)
            {
                while (scaledNumberOfBytes >= increment)
                {
                    suffixIndex++;
                    scaledNumberOfBytes /= increment;
                }

                if (Math.Abs(scaledNumberOfBytes - 0) < 0.1)
                {
                    scaledNumberOfBytes = 1;
                }
            }

            return String.Format("{0}{1}", scaledNumberOfBytes.ToString("0.#"), Suffixes[suffixIndex]);
        }
    }
}