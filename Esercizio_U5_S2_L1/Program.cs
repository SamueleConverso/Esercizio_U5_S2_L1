using Esercizio_U5_S2_L1.Data;
using Microsoft.EntityFrameworkCore;
using Esercizio_U5_S2_L1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Esercizio_U5_S2_L1.Models;
using FluentEmail.MailKitSmtp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionstring)
);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options => {
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.Cookie.Name = "GestionaleStudenti";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
});

builder.Services.AddFluentEmail(builder.Configuration.GetSection("MailSettings").GetValue<string>("FromDefault"))
    .AddRazorRenderer()
    .AddMailKitSender(new SmtpClientOptions() {
        Server = builder.Configuration.GetSection("MailSettings").GetValue<string>("Server"),
        Port = builder.Configuration.GetSection("MailSettings").GetValue<int>("Port"),
        User = builder.Configuration.GetSection("MailSettings").GetValue<string>("Username"),
        Password = builder.Configuration.GetSection("MailSettings").GetValue<string>("Password"),
        UseSsl = builder.Configuration.GetSection("MailSettings").GetValue<bool>("UseSsl"),
        RequiresAuthentication = builder.Configuration.GetSection("MailSettings").GetValue<bool>("RequiresAuthentication"),
    });

builder.Services.AddScoped<StudenteService>();
builder.Services.AddScoped<LoggerService>();
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<ApplicationRole>>();
builder.Services.AddScoped<EmailService>();

LoggerService.ConfigureLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
