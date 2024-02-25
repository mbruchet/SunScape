namespace SunScape.Options;

public class SmtpEmailServiceSettings
{
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; } = 25;
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public bool EnableSsl { get; set; } = false;
}
