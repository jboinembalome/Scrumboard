using AutoFixture;
using AutoFixture.Kernel;
using Scrumboard.Domain.Teams;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Teams;

public sealed class MemberIdCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new MemberIdSpecimenBuilder());
    }
}

internal sealed class MemberIdSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(MemberId))
        {
            return new NoSpecimen();
        }

        var guid = context.Create<Guid>();
        
        return new MemberId(guid.ToString());
    }
}
