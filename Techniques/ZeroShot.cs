using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using TextMind.Configuration;

namespace TextMind.Techniques;

public class ZeroShot : IPromptingTechnique
{
    private readonly IChatClient _client;
    private readonly AssistantSettings _assistant;

    public ZeroShot(IChatClient client, IOptions<AssistantSettings> assistant)
    {
        _client = client;
        _assistant = assistant.Value;
    }

    public string Name => "Zero-shot";

  public async Task<string> ExecuteAsync(string userQuery, CancellationToken cancellationToken = default)
{
        var prompt = $@"
Eres un asistente académico experto en {_assistant.Domain}.

Tu tarea es:
- Comprender el texto
- Explicar las ideas principales de forma sencilla
- Dar un resumen claro y breve0

Texto:
{userQuery}
";

        var response = await _client.GetResponseAsync(prompt);
        return response.ToString();
    }
}