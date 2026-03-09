using Microsoft.AspNetCore.Http;

namespace RequirementAI.Business.Helpers;

public static class BinaryHelper
{
    public static byte[] GetBytes(IFormFile file)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        return ms.ToArray();
    }
}