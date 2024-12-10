namespace LifeTimeBot.App.Options;

public class AppOptions
{
    public AsrServiceOptions AsrService { get; set; }
    public LlmServiceOptions LlmHuggingFaceService { get; set; }
    public AppDbConnections DbConnections { get; set; }
    public CorsOptions Cors { get; set; }
}