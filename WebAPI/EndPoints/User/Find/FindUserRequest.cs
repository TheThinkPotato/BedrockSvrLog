using Microsoft.AspNetCore.Mvc;

namespace WebAPI;

public class FindUserRequest
{
    [FromQuery]
    public string Name { get; init; } = string.Empty;
}
