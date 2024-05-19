using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Scrumboard.Web.Controllers;

/// <summary>
/// Base controller that must be used on every controller working with MediatR.
/// </summary>
/// <remarks>Avoid adding the mediator in each controller constructor.</remarks>
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
