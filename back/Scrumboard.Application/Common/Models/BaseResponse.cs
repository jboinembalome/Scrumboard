
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Common.Models;

public class BaseResponse
{
    public BaseResponse()
    {
        Success = true;
    }
    public BaseResponse(string? message = null)
    {
        Success = true;
        Message = message;
    }

    public BaseResponse(string? message, bool success)
    {
        Success = success;
        Message = message;
    }

    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<string> ValidationErrors { get; set; }
}
