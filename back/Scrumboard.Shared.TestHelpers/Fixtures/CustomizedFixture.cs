using AutoFixture;

namespace Scrumboard.Shared.TestHelpers.Fixtures;

public sealed class CustomizedFixture : Fixture
{
    private static readonly Lazy<Type[]> CustomizationTypes = new(GetCustomizationTypes);

    public CustomizedFixture()
    {
        ApplyCustomizations(CustomizationTypes.Value);
    }

    public CustomizedFixture(params Type[] localCustomizationTypes)
        : this()
    {
        ApplyCustomizations(localCustomizationTypes);
    }

    private void ApplyCustomizations(params Type[] customizationTypes)
    {
        foreach (var localCustomizationType in customizationTypes)
        {
            if (!typeof(ICustomization).IsAssignableFrom(localCustomizationType))
            {
                throw new ArgumentException($"Type {localCustomizationType.Name} does not implement ICustomization", nameof(customizationTypes));
            }

            var customization = (ICustomization?)Activator.CreateInstance(localCustomizationType);
            Customize(customization);
        }
    }

    private static Type[] GetCustomizationTypes()
    {
        var excludedAssemblies = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Microsoft",
            "System"
        };

        var customizationTypes =
            from a in AppDomain.CurrentDomain.GetAssemblies()
            where !a.IsDynamic &&
                  !string.IsNullOrWhiteSpace(a.FullName) &&
                  !excludedAssemblies.Contains(a.FullName.Split('.')[0])
            from t in a.GetExportedTypes()
            where !t.IsAbstract && !t.ContainsGenericParameters && typeof(IAutoAppliedCustomization).IsAssignableFrom(t)
            select t;

        return customizationTypes.ToArray();
    }
}
