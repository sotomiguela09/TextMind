using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TextMind.Configuration;
using TextMind.Models;
using TextMind.Techniques;

namespace TextMind.Controllers;

public class HomeController : Controller
{
    private readonly IEnumerable<IPromptingTechnique> _techniques;
    private readonly OllamaSettings _ollamaSettings;

    public HomeController(IEnumerable<IPromptingTechnique> techniques, IOptions<OllamaSettings> ollamaSettings)
    {
        _techniques = techniques;
        _ollamaSettings = ollamaSettings.Value;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = BuildModel();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(string selectedTechnique, string query)
    {
        var model = BuildModel();
        model.SelectedTechnique = selectedTechnique;
        model.Query = query;

        if (string.IsNullOrWhiteSpace(query))
        {
            model.Error = "Debe escribir una consulta.";
            return View(model);
        }

        var technique = _techniques.FirstOrDefault(t => t.Name == selectedTechnique);
        if (technique is null)
        {
            model.Error = "Técnica no encontrada.";
            return View(model);
        }

        try
        {
            model.Response = await technique.ExecuteAsync(query);
        }
        catch (Exception ex)
        {
            model.Error = ex.Message;
        }

        return View(model);
    }

    private TextMindViewModel BuildModel()
    {
        return new TextMindViewModel
        {
            Model = _ollamaSettings.Model,
            BaseUrl = _ollamaSettings.BaseUrl,
            AvailableTechniques = _techniques.Select(t => t.Name).ToList(),
            SelectedTechnique = _techniques.First().Name
        };
    }
}
