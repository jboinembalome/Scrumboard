using System.Reflection;
using Scrumboard.Shared.TestHelpers.Serializations;

namespace Scrumboard.Shared.TestHelpers.Extensions;

public static class AssemblyExtensions
{
    public static T? LoadResourceJson<T>(this Assembly assembly, string resourceName)
    {
        var json = LoadStringFromEmbeddedResource(assembly, resourceName);

        return SerializationHelper.Deserialize<T>(json);
    }

    public static string LoadJsonResource(this Assembly assembly, string resourceName)
    {
        var json = LoadStringFromEmbeddedResource(assembly, resourceName);

        return SerializationHelper.Reserialize(json);
    }

    private static string LoadStringFromEmbeddedResource(Assembly assembly, string resourceFullName)
    {
        using Stream stream = assembly.GetManifestResourceStream(resourceFullName) 
                              ?? throw new ArgumentException($"resource `{resourceFullName}` not found.");
        
        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
