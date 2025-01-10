using Frontoffice.Models;
using Frontoffice.Services;
using Microsoft.Extensions.FileProviders;

namespace Frontoffice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<SharedFileService>();
            builder.Services.AddSingleton<CustomerService>();
            builder.Services.AddSingleton<SpaceService>();
            builder.Services.AddSingleton<BookingService>();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Expiration
                options.Cookie.HttpOnly = true;                // Security
                options.Cookie.IsEssential = true;             // Need for essential cookie
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
