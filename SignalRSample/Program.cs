using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalRSample.Data;
using SignalRSample.Hubs;

namespace SignalRSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var connectionAzureSignalR = "Endpoint=https://kosmetixsignalr.service.signalr.net;AccessKey=fGsbl1ZLp5eA2TLWpFZhBW3ypWbU1KovNCMT/znFOEg=;Version=1.0;";
            builder.Services.AddSignalR().AddAzureSignalR(connectionAzureSignalR);

            var app = builder.Build(); 

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.MapHub<UserHub>("/hubs/userCount");
            app.MapHub<DeathlyHallowsHub>("/hubs/deathlyhallows");
            app.MapHub<HouseGroupHub>("/hubs/houseGroup");
            app.MapHub<NotificationHub>("/hubs/notification");
            app.MapHub<BasicChatHub>("/hubs/basicchat");
            app.MapHub<OrderHub>("/hubs/order");
            app.MapHub<ChatHub>("/hubs/chat");

            app.Run();
        }
    }
}