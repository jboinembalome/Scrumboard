﻿using Microsoft.AspNetCore.Identity;
using System.Linq;
using Scrumboard.Application.Common.Models;

namespace Scrumboard.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }
}