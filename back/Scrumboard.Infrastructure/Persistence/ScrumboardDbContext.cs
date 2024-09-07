﻿using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Identity;

namespace Scrumboard.Infrastructure.Persistence;

public class ScrumboardDbContext(
    DbContextOptions options)
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        builder.ApplyConfigurationsFromAssembly(assembly);
        builder.ApplyModelConfigurationsFromAssembly(assembly);
        
        base.OnModelCreating(builder);
    }
}
