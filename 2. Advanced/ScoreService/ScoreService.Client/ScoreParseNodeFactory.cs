using Microsoft.Kiota.Abstractions.Serialization;

namespace ScoreService.Client;

public class ScoreParseNodeFactory : IParseNodeFactory
{
    public IParseNode GetRootParseNode(string contentType, Stream content)
    {
        throw new NotImplementedException();
    }

    public string ValidContentType => throw new NotImplementedException();
}