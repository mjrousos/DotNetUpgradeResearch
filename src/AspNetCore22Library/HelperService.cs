using Microsoft.AspNetCore.Http;

public static class HelperService
{
    public static string GetUserAgent(HttpContext context)
    {
        return context.Request.Headers["User-Agent"];
    }
}