using AutoFixture;
using AutoFixture.Kernel;
using Scrumboard.Domain.Cards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Cards;

public sealed class AssigneeIdCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new AssigneeIdSpecimenBuilder());
    }
}

internal sealed class AssigneeIdSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(AssigneeId))
        {
            return new NoSpecimen();
        }

        var guid = context.Create<Guid>();
        
        return new AssigneeId(guid.ToString());
    }
}
