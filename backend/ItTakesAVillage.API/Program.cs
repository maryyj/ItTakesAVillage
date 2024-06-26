namespace ItTakesAVillage.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString;

            if (builder.Environment.IsDevelopment())
            {
                connectionString = builder.Configuration.GetConnectionString("ItTakesAVillageContextConnection") ?? throw new InvalidOperationException("Connection string 'ItTakesAVillageContextConnection' not found.");
            }
            else
            {
                connectionString = Environment.GetEnvironmentVariable("ItTakesAVillageContextConnection") ?? throw new InvalidOperationException("Environment variable 'ItTakesAVillageContextConnection' not found or is null.");
            }

            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IGroupChatService, GroupChatService>();
            builder.Services.AddScoped<IEventService<DinnerInvitation>, DinnerInvitationService>();
            builder.Services.AddScoped<IEventService<PlayDate>, PlayDateService>();
            builder.Services.AddScoped<IEventService<ToolPool>, ToolPoolService>();
            builder.Services.AddScoped<IEventService<ToolLoan>, ToolLoanService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IRepository<Group>, EFRepository<Group>>();
            builder.Services.AddScoped<IRepository<UserGroup>, EFRepository<UserGroup>>();
            builder.Services.AddScoped<IRepository<ItTakesAVillageUser>, EFRepository<ItTakesAVillageUser>>();
            builder.Services.AddScoped<IRepository<Notification>, EFRepository<Notification>>();
            builder.Services.AddScoped<IRepository<DinnerInvitation>, EFRepository<DinnerInvitation>>();
            builder.Services.AddScoped<IRepository<PlayDate>, EFRepository<PlayDate>>();
            builder.Services.AddScoped<IRepository<ToolPool>, EFRepository<ToolPool>>();
            builder.Services.AddScoped<IRepository<ToolLoan>, EFRepository<ToolLoan>>();
            builder.Services.AddScoped<IRepository<GroupChat>, EFRepository<GroupChat>>();

            builder.Services.AddDbContext<ItTakesAVillageContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddDefaultIdentity<ItTakesAVillageUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ItTakesAVillageContext>();

            //builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.MapRazorPages();
            app.MapControllers();
            app.Run();
        }
    }
}
