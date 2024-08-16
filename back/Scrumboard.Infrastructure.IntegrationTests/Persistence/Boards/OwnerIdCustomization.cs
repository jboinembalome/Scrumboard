using AutoFixture;
using AutoFixture.Kernel;
using Scrumboard.Domain.Boards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards;

public sealed class OwnerIdCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new OwnerIdSpecimenBuilder());
    }
}

internal sealed class OwnerIdSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(OwnerId))
        {
            return new NoSpecimen();
        }

        var guid = context.Create<Guid>();
        
        return new OwnerId(guid.ToString());
    }
}
