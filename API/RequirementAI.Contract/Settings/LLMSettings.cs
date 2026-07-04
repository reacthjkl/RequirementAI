namespace RequirementAI.Contract.Settings;

public class LLMSettings
{
    public Dictionary<string, LLMProviderSettings> Providers { get; set; } = [];
    public LLMRoutingSettings Routing { get; set; } = new();
}

public class LLMProviderSettings
{
    public string Type { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public Dictionary<string, string> Models { get; set; } = [];
}

public class LLMRoutingSettings
{
    public LLMRouteSettings Refinement { get; set; } = new();
    public LLMRouteSettings Analysis { get; set; } = new();
}

public class LLMRouteSettings
{
    public string Provider { get; set; } = null!;
    public string Model { get; set; } = null!;
}
