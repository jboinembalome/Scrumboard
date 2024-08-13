using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Scrumboard.SharedKernel.Entities;

namespace Scrumboard.Infrastructure.Persistence;

internal static class ModelBuilderExtensions
{
    private static readonly MethodInfo ConfigureModelMethodInfo =
        typeof(IModelConfiguration).GetMethod(
            nameof(IModelConfiguration.ConfigureModel),
            BindingFlags.Public | BindingFlags.Instance)
        ?? throw new MissingMethodException(
            nameof(IModelConfiguration), 
            nameof(IModelConfiguration.ConfigureModel));

    public static void UseSequence<TEntity, TId>(
        this ModelBuilder modelBuilder, 
        int increment = 100)
        where TEntity : EntityBase<TId>
        where TId : struct, IEquatable<TId>
    {
        var entityBuilder = modelBuilder.Entity<TEntity>();

        var schema = entityBuilder.Metadata.GetSchema();
        var tableName = entityBuilder.Metadata.GetTableName();
        var sequenceName = $"{tableName}_HiLoSequence";

        modelBuilder
            .HasSequence(sequenceName, schema)
            .IncrementsBy(increment);

        entityBuilder
            .Property(e => e.Id)
            .UseHiLo(sequenceName, schema);
    }

    public static ModelBuilder ApplyModelConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
    {
        var daoConfigurationTypes = assembly.DefinedTypes
            .Where(type => type.GetInterfaces().Any(i => i.UnderlyingSystemType == typeof(IModelConfiguration)));

        foreach (var type in daoConfigurationTypes)
        {
            ConfigureModelMethodInfo.Invoke(
                obj: Activator.CreateInstance(type),
                parameters: [modelBuilder]);
        }

        return modelBuilder;
    }
}
