namespace ItTakesAVillage.Frontend;

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
        builder.Services.AddScoped<IRepository<ItTakesAVillageUser>,EFRepository <ItTakesAVillageUser>>();
        builder.Services.AddScoped<IHttpService, HttpService>();

        builder.Services.AddDbContext<ItTakesAVillageContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<ItTakesAVillageUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ItTakesAVillageContext>();

        // Add services to the container.
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}
