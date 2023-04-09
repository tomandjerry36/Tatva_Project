using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using CI_PlatForm.Entities.Data;
using CI_PlatForm.Repository.Interface;
using CI_PlatForm.Repository.Repositories;


        var builder = WebApplication.CreateBuilder(args);
        /*builder.Services.AddDbContext<CiplatformContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));*/

        // Add services to the container.

        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<CiplatformContext>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMissionRepository, MissionRepository>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();
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

        /*app.UseAuthentication();*/

        app.UseAuthorization();
        app.UseSession();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=User}/{action=Index}/{id?}");

        app.Run();
    