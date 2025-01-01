using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Backoffice.Data;
using Backoffice.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace Backoffice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<SpaceBookContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SpaceBookContext") ?? throw new InvalidOperationException("Connection string 'SpaceBookContext' not found.")));

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddSingleton<SharedFileService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login";
                //options.AccessDeniedPath = "/AccessDenied";
            });

            //builder.Services.ConfigureApplicationCookie(options => { options.LoginPath = "/Login/Index"; });

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<SpaceBookContext>();
                context.Database.EnsureCreated();
                // DbInitializer.Initialize(context);
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var sharedFileService = new SharedFileService(builder.Configuration);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(sharedFileService.GetSharedFilesDirectory()),
                RequestPath = "/SharedFiles"
            });

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
