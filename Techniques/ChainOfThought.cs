using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using TextMind.Configuration;

namespace TextMind.Techniques;

public class ChainOfThought : IPromptingTechnique
{
    private readonly IChatClient _client;
    private readonly AssistantSettings _assistant;

    public ChainOfThought(IChatClient client, IOptions<AssistantSettings> assistant)
    {
        _client = client;
        _assistant = assistant.Value;
    }

    public string Name => "Chain-of-Thought";

    public async Task<string> ExecuteAsync(string userQuery, CancellationToken cancellationToken = default)
    {
       var prompt = $$"""
Eres un asistente académico experto en {{_assistant.Domain}}.

Para la siguiente consulta, analiza el contenido paso a paso y luego responde.

Estructura tu respuesta así:

Análisis:
- Identifica las ideas principales del texto

Explicación:
- Explica cada idea de forma sencilla

Resumen:
- Da un resumen claro y breve

Texto:
{{userQuery}}
""";

        var response = await _client.GetResponseAsync(prompt, cancellationToken: cancellationToken);
        return response.Text ?? string.Empty;
    }
}
