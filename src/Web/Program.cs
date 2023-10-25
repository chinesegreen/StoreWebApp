using Infrastructure.Options;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Configuration;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddMvc();

if (builder.Environment.IsDevelopment() || builder.Environment.EnvironmentName == "Docker")
{
    Infrastructure.Dependencies.ConfigureServices(builder.Configuration, builder.Services);
}
else
{
    builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("IdentityConnection");
        options.UseNpgsql(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
    });
    builder.Services.AddDbContext<CatalogContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("CatalogConnection");
        options.UseNpgsql(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
    });
}

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Tokens.ProviderMap.Add("CustomEmailConfirmation",
        new TokenProviderDescriptor(
            typeof(CustomEmailConfirmationTokenProvider<ApplicationUser>)));
    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

}).AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultUI()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();
builder.Services.AddIdentityOptions();

builder.Services.AddCookieSettings();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

//builder.Services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddWebServices(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.Logger.LogInformation("Adding Development middleware...");
    app.UseDeveloperExceptionPage();
}
else
{
    app.Logger.LogInformation("Adding non-Development middleware...");
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapRazorPages();

app.Run();
