using Nhea.Communication;
using Nhea.Configuration.GenericConfigSection.Communication;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nhea.CoreCommunicationService
{
    // Source generated metadata keeps JSON deserialization working with PublishTrimmed.
    [JsonSerializable(typeof(IEnumerable<SmtpElement>))]
    [JsonSerializable(typeof(MailParameters))]
    internal partial class CommunicationJsonContext : JsonSerializerContext
    {
    }
}
