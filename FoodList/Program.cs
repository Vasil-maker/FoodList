using FoodList.DAL;
using FoodList.DAL.Interfaces;
using FoodList.DAL.Repositories;
using FoodList.Domain.Entity;
using FoodList.Domain.Response;
using FoodListSerivce.Implementations;
using FoodListSerivce.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IBaseRepository<FoodCulEntity>, FoodCulRepository>();
            builder.Services.AddScoped<IFoodCulSerivce, FoodCulSerivce>();

            var connectionString = builder.Configuration.GetConnectionString("MSSQL");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=FoodCul}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
