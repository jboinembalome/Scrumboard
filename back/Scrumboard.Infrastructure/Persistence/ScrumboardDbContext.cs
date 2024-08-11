using Microsoft.EntityFrameworkCore;
using Scrumboard.Infrastructure.Identity;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.SharedKernel.Entities;

namespace Scrumboard.Infrastructure.Persistence;

public class ScrumboardDbContext(
    DbContextOptions options,
    ICurrentUserService currentUserService,
    ICurrentDateService currentDateService)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Activity> Activities { get; set; }
    
    public DbSet<Board> Boards { get; set; }
    public DbSet<BoardSetting> BoardSettings { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<ListBoard> ListBoards { get; set; }
    public DbSet<Team> Teams { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<ICreatedAtEntity>())
        {
            // TODO: Use interceptors
            // TODO: Hack to not depend on httpContextAccessor when using ScrumboardDbContextSeed
            // (Will be removed later)
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (entry.State is EntityState.Added && entry.Entity.CreatedBy.Value is null)
            {
                entry.Entity.CreatedBy = (UserId)currentUserService.UserId;
                entry.Entity.CreatedDate = currentDateService.Now;
                break;
            }
        }
        
        foreach (var entry in ChangeTracker.Entries<IModifiedAtEntity>())
        {
            // TODO: Use interceptors
            if (entry.State is EntityState.Modified)
            {
                entry.Entity.LastModifiedBy = (UserId)currentUserService.UserId;
                entry.Entity.LastModifiedDate = currentDateService.Now;
                break;
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
