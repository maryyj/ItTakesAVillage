namespace ItTakesAVillage.Infrastructure.DAL;
public class ItTakesAVillageContext : IdentityDbContext<ItTakesAVillageUser>
{
    public DbSet<BaseEvent> Events { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ToolLoan> ToolLoans { get; set; }
    public DbSet<GroupChat> GroupChats { get; set; }

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
       .HasValue<PlayDate>("PlayDate")
       .HasValue<ToolPool>("ToolPool");

        builder.Entity<GroupChat>().Navigation(x => x.Group).AutoInclude();
        builder.Entity<UserGroup>().Navigation(x => x.Group).AutoInclude();
        builder.Entity<UserGroup>().Navigation(x => x.User).AutoInclude();
        builder.Entity<Notification>().Navigation(x => x.RelatedEvent).AutoInclude();
        builder.Entity<ToolLoan>().Navigation(x => x.ToolPool).AutoInclude();
    }
}
