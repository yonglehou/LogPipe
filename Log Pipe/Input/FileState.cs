using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Consortio.Services.LogPipe.Input {
    public class FileState {
        private Dictionary<string, long> state;

        public void Initialize() {
            RestoreFromDisk();
        }
        
        private void RestoreFromDisk() {
            try {
                var content = File.ReadAllText(Filename);
                if(!string.IsNullOrWhiteSpace(content)) {
                    state = JsonConvert.DeserializeObject<Dictionary<string, long>>(content);
                } else {
                    state = new Dictionary<string, long>();
                }
            } catch(Exception) {
                state = new Dictionary<string, long>();
            }
        }

        private string Filename {
            get { return "file.db"; }
        }

        private void PersistToDisk() {
            var content = JsonConvert.SerializeObject(state);
            File.WriteAllText(Filename, content);
        }

        public long this[string path, string type] {
            get {
                long position;
                return state.TryGetValue(path + "-" + type, out position) ? position : 0;
            }
            set {
                lock (this) {
                    state[path + "-" + type] = value;
                    PersistToDisk();
                }
            }
        }

        public void Remove(string path, string type) {
            state.Remove(path + "-" + type);
        }
    }
}