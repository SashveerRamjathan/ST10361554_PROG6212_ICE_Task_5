using ST10361554_PROG6212_ICE_Task_5.Models;

namespace ST10361554_PROG6212_ICE_Task_5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register the MovieManager service
            builder.Services.AddSingleton<MovieManager>();

            // Add session services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout to 30 minutes
                options.Cookie.HttpOnly = true; // Set the session cookie to be HTTP only
                options.Cookie.IsEssential = true; // Set the session cookie to be essential
            });

            var app = builder.Build();

            // Call the method here on application start-up
            var movieManager = app.Services.GetRequiredService<MovieManager>();
            movieManager.GenerateShowtimes(); // Ensure this runs only once at start-up

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Add session middleware
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
