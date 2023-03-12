using Geo.Monitoring.Blazor.Services;
using Geo.Monitoring.Blazor.Services.Geo;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);


builder.Services.AddRazorPages(options =>
{
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<AuthenticationStateProvider, ReValidatingIdentityAuthenticationStateProvider>();
builder.Services.AddScoped<ILoginUserService, LoginUserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddGeoServiceOptions(builder.Configuration);

builder.Services.AddGeoServiceClientMock();
//builder.Services.AddGeoServiceClient();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDevExpressBlazor(options =>
{
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
    options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});
builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();


//builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\Users\Sergey\AppData\Roaming\ASP.NET\Https\ggg"))
//    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
//    {
//        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
//        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
//    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


app.Run();