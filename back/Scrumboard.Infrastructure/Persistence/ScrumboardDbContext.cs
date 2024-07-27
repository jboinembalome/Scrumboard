using Microsoft.EntityFrameworkCore;
using Scrumboard.Infrastructure.Identity;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Cards;
using Scrumboard.Infrastructure.Persistence.Cards.Activities;
using Scrumboard.Infrastructure.Persistence.Cards.Checklists;
using Scrumboard.Infrastructure.Persistence.Cards.Comments;
using Scrumboard.Infrastructure.Persistence.Cards.Labels;
using Scrumboard.Infrastructure.Persistence.ListBoards;
using Scrumboard.Infrastructure.Persistence.Teams;

namespace Scrumboard.Infrastructure.Persistence;

public class ScrumboardDbContext(
    DbContextOptions options,
    ICurrentUserService currentUserService,
    IDateTime dateTime)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ActivityDao> Activities { get; set; }
    
    public DbSet<BoardDao> Boards { get; set; }
    public DbSet<BoardSettingDao> BoardSettings { get; set; }
    public DbSet<CardDao> Cards { get; set; }
    public DbSet<ChecklistDao> Checklists { get; set; }
    public DbSet<ChecklistItemDao> ChecklistItems { get; set; }
    public DbSet<CommentDao> Comments { get; set; }
    public DbSet<LabelDao> Labels { get; set; }
    public DbSet<ListBoardDao> ListBoards { get; set; }
    public DbSet<TeamDao> Teams { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            // TODO: Use interceptors
            switch (entry)
            {
                case { State: EntityState.Added }:
                    // TODO: Hack to not depend on httpContextAccessor when using ScrumboardDbContextSeed
                    // (Will be removed later)
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                    if (entry.Entity.CreatedBy is null)
                    {
                        entry.Entity.CreatedBy = currentUserService.UserId;
                        entry.Entity.CreatedDate = dateTime.Now;
                    }
                    break;
                case { State: EntityState.Modified }:
                    entry.Entity.LastModifiedBy = currentUserService.UserId;
                    entry.Entity.LastModifiedDate = dateTime.Now;
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
