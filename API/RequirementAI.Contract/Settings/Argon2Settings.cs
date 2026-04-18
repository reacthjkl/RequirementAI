namespace RequirementAI.Contract.Settings;

public class Argon2Settings
{
    public int TimeCost { get; set; } = 4;
    public int MemoryCost { get; set; } = 65536;
    public int Lanes { get; set; } = 4;
    public int SaltLength { get; set; } = 16;
}