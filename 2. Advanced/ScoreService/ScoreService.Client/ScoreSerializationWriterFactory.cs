using Microsoft.Kiota.Abstractions.Serialization;

namespace ScoreService.Client;

public class ScoreSerializationWriterFactory : ISerializationWriterFactory
{
    public ISerializationWriter GetSerializationWriter(string contentType)
    {
        throw new NotImplementedException();
    }

    public string ValidContentType => throw new NotImplementedException();
}