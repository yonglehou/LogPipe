using System;
using System.IO;
using System.Text;

namespace Consortio.Services.LogPipe.Input {
    public class FileReader : IDisposable {
        private readonly long basePosition;
        private readonly FileStream fileStream;
        private readonly StreamReader streamReader;
        private readonly TrackingTextReader trackingTextReader;

        public FileReader(string path, long basePosition, Encoding encoding) {
            this.basePosition = basePosition;
            fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            streamReader = new StreamReader(fileStream, encoding);
            trackingTextReader = new TrackingTextReader(streamReader);

            fileStream.Position = basePosition;
        }

        public long Position {
            get { return basePosition + trackingTextReader.Position; }
        }

        public string ReadLine() {
            try {
                return trackingTextReader.ReadLine();
            } catch {
                return null;
            }
        }

        public void Dispose() {
            trackingTextReader.Dispose();
            streamReader.Dispose();
            fileStream.Dispose();
        }
    }
}