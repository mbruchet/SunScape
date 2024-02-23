namespace SunScape.Services;

public class CultureService
{
    public List<string> GetSupportedCultures() => new List<string>
        {
            "en-US", "es-ES", "fr-FR", "de-DE", "it-IT", "ln-CG"
        };
}
