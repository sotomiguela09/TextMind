using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using TextMind.Configuration;

namespace TextMind.Techniques;

public class FewShot : IPromptingTechnique
{
    private readonly IChatClient _client;
    private readonly AssistantSettings _assistant;

    public FewShot(IChatClient client, IOptions<AssistantSettings> assistant)
    {
        _client = client;
        _assistant = assistant.Value;
    }

    public string Name => "Few-shot";

    public async Task<string> ExecuteAsync(string userQuery, CancellationToken cancellationToken = default)
    {
        var messages = new List<ChatMessage>

{
    new(ChatRole.System, $"Eres un asistente académico experto en {_assistant.Domain}. Explica de forma clara y en máximo 3 puntos."),

    new(ChatRole.User, "¿Qué es la fotosíntesis?"),
    new(ChatRole.Assistant, "1. Es el proceso por el cual las plantas producen su alimento. 2. Usan luz solar, agua y dióxido de carbono. 3. Generan oxígeno como resultado."),

    new(ChatRole.User, "¿Qué es la gravedad?"),
    new(ChatRole.Assistant, "1. Es una fuerza que atrae los objetos hacia el centro de la Tierra. 2. Es responsable de que no flotemos. 3. Fue explicada por Newton."),

    new(ChatRole.User, userQuery)
};
        var response = await _client.GetResponseAsync(messages, cancellationToken: cancellationToken);
        return response.Text ?? string.Empty;
    }
}
