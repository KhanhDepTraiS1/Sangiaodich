using Microsoft.AspNetCore.Mvc;
using Sangiaodich;
using Sangiaodich.DBContext;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<BackgroundServicesGetPrice>();
builder.Services.AddDbContextFactory<DBSetIdentityDBContext>(options => options
    .EnableServiceProviderCaching(true)
    .EnableSensitiveDataLogging(true)
    .EnableThreadSafetyChecks(true)
    .EnableDetailedErrors(true));
var app = builder.Build();
Process[] chromeInstances = Process.GetProcessesByName("chrome");
foreach (Process p in chromeInstances) p.Kill();
using var DbContext = app.Services.CreateScope();
using var Context = DbContext.ServiceProvider.GetRequiredService<DBSetIdentityDBContext>();
if (!Context.Database.CanConnect())
{
    await Context.Database.EnsureCreatedAsync();
}
var RunServices = app.Services.GetRequiredService<BackgroundServicesGetPrice>();
_=Task.Run(async () =>
{
    await RunServices.ExecuteAsync(Context);
});
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
app.MapGet("/api/GetPrice", (DBSetIdentityDBContext DB, [FromQuery] string SGD, [FromQuery] string MCK) =>
{
    List<Sangiaodich.Model.MCK> LstMCK = [];
    var FindMCK = DB.MCK.Where(x => x.SGD == SGD && x.MCKName == MCK).FirstOrDefault();
    return Results.Ok(FindMCK);
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
