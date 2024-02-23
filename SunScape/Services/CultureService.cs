namespace SunScape.Services;

// TODO 1. Create a culture service to retrieve the list of cultures
public class CultureService
{
    public List<string> GetSupportedCultures() => new List<string>
        {
            "en-US", "es-ES", "fr-FR", "de-DE", "it-IT", "ln-CG"
        };
}
