using AutoFixture;
using AutoFixture.Kernel;
using Scrumboard.Domain.Common;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Users;

public sealed class UserIdAppliedCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new UserIdSpecimenBuilder());
    }
}

internal sealed class UserIdSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(UserId))
        {
            return new NoSpecimen();
        }

        var guid = context.Create<Guid>();
        
        return new UserId(guid.ToString());
    }
}
