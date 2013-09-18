using System.Collections.Generic;

namespace Consortio.Services.LogPipe.Configuration.Input {
    public interface IInputConfiguration : ITypeConfiguration {
        HashSet<string> Tags { get; }
    }
}