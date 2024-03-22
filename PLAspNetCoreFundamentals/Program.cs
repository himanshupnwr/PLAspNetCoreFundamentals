using Microsoft.EntityFrameworkCore;
using PLAspNetCoreFundamentals.Models;
using PLAspNetCoreFundamentals.App;
using Microsoft.AspNetCore.Identity;

namespace PLAspNetCoreFundamentals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("PieShopDbContextConnection") ?? throw new InvalidOperationException("Connection string 'PieShopDbContextConnection' not found.");

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            //Injecting Dependencies
            //builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
            //builder.Services.AddScoped<IPieRepository, MockPieRepository>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IPieRepository, PieRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddRazorPages();
            builder.Services.AddRazorComponents().AddInteractiveServerComponents();

            builder.Services.AddDbContext<PieShopDbContext>(options => {
                options.UseSqlServer(
                    builder.Configuration["ConnectionStrings:PieShopDbContextConnection"]);
            });

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<PieShopDbContext>();

            var app = builder.Build();

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

            app.UseAuthorization();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapDefaultControllerRoute();
                //name: "default",
                //pattern: "{controller=Home}/{action=Index}/{id?}");
            app.UseAntiforgery();
            app.MapRazorPages();
            //app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
            DbInitializer.Seed(app);

            app.Run();
        }
    }
}
