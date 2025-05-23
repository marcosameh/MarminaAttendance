﻿using App.Core.Entities;
using App.Core.Infrastrcuture;
using App.Core.Managers;
using App.UI.InfraStructure;
using AppCore.Infrastructure;
using Hangfire;
using MarminaAttendance.Identity;
using MarminaAttendanceAPI.Endpoints;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<MarminaAttendanceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{

    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+أبتثجحخدذرزسشصضطظعغفقكلمنهويىئءآإةؤا ";
    options.ClaimsIdentity.UserIdClaimType = "UserID";
}).AddEntityFrameworkStores<IdentityContext>().AddDefaultUI().AddDefaultTokenProviders();

builder.Services.Configure<FormOptions>(options =>
{
    //options.ValueLengthLimit = int.MaxValue;
    options.ValueCountLimit = int.MaxValue;
    //options.BufferBodyLengthLimit = int.MaxValue;
    //options.MultipartBodyLengthLimit = int.MaxValue;
});


builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login");

builder.Services.AddScoped<ClassManager>();
builder.Services.AddScoped<WeekManager>();
builder.Services.AddScoped<ServantManager>();
builder.Services.AddScoped<TimeManager>();
builder.Services.AddScoped<ServedManager>();
builder.Services.AddScoped<EmailManager>();
builder.Services.AddScoped<ExcelProcessor>();
builder.Services.AddScoped<QrCodeService>();
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();

builder.Services.AddHangfire(configuration => configuration
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                  .UseSimpleAssemblyNameTypeSerializer()
                  .UseRecommendedSerializerSettings()
                  .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (ctx.File.Name.EndsWith(".png")) // Apply only for images (QR codes)
        {
            ctx.Context.Response.Headers.Append("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
            ctx.Context.Response.Headers.Append("Pragma", "no-cache");
            ctx.Context.Response.Headers.Append("Expires", "0");
        }
    }
});

app.UseRouting();
app.ConfigureResponsibleServantEndpoints();
app.UseAuthentication();

app.UseAuthorization();
app.UseHangfireDashboard("/hangfire-marmina", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.MapGet("/", context =>
{
    context.Response.Redirect("/classes/list");
    return Task.CompletedTask;
});

app.MapRazorPages();


app.Run();
