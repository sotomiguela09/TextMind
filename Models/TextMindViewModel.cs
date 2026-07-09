namespace TextMind.Models;

public class TextMindViewModel
{
    public string Model { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public List<string> AvailableTechniques { get; set; } = new();
    public string SelectedTechnique { get; set; } = string.Empty;
    public string Query { get; set; } = string.Empty;
    public string? Response { get; set; }
    public string? Error { get; set; }
}
