using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using TextMind.Configuration;

namespace TextMind.Techniques;

public class RolePrompting : IPromptingTechnique
{
    private readonly IChatClient _client;
    private readonly AssistantSettings _assistant;

    public RolePrompting(IChatClient client, IOptions<AssistantSettings> assistant)
    {
        _client = client;
        _assistant = assistant.Value;
    }

    public string Name => "Role Prompting";

    public async Task<string> ExecuteAsync(string userQuery, CancellationToken cancellationToken = default)
    {
        var systemPrompt = $$"""
Actúa como un experto en análisis y comprensión de textos en {{_assistant.Domain}}.

Tu estilo de comunicación:
- Explicas de forma clara y sencilla.
- Simplificas conceptos complejos.
- Das respuestas estructuradas y fáciles de entender.
- Puedes resumir y explicar información.

Tu objetivo es ayudar al usuario a comprender mejor el contenido proporcionado.
""";
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, systemPrompt),
            new(ChatRole.User, userQuery)
        };

        var response = await _client.GetResponseAsync(messages, cancellationToken: cancellationToken);
        return response.Text ?? string.Empty;
    }
}
