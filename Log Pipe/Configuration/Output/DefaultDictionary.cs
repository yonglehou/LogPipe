using System.Collections.Generic;

namespace Consortio.Services.LogPipe.Configuration.Output {
    public class DefaultDictionary<TKey, TValue> {
        private readonly Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        public DefaultDictionary() {}

        public DefaultDictionary(TValue defaultValue) {
            DefaultValue = defaultValue;
        }

        public TValue DefaultValue { get; set; }

        public TValue this[TKey key] {
            get {
                TValue value;
                if (dictionary.TryGetValue(key, out value))
                    return value;
                
                return DefaultValue;
            }
            set { dictionary[key] = value; }
        }
    }
}