using ItTakesAVillage.Contracts;
using ItTakesAVillage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ItTakesAVillage.Data;

public class ItTakesAVillageContext : IdentityDbContext<ItTakesAVillageUser>
{
    public DbSet<Models.BaseEvent> Events { get; set; }
    public DbSet<Models.Group> Groups { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    public ItTakesAVillageContext(DbContextOptions<ItTakesAVillageContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<BaseEvent>()
       .ToTable("Events")
       .HasDiscriminator<string>("EventType")
       .HasValue<BaseEvent>("BaseEvent")
       .HasValue<DinnerInvitation>("DinnerInvitation")
       .HasValue<PlayDate>("PlayDate");

        builder.Entity<UserGroup>().Navigation(x => x.Group).AutoInclude();
        builder.Entity<UserGroup>().Navigation(x => x.User).AutoInclude();
        builder.Entity<Notification>().Navigation(x => x.RelatedEvent).AutoInclude();
    }
}
