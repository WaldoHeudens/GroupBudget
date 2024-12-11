using GB_Web.Services;
using GroupBudget_Web.Data;
using GroupBudget_Web.Models;
using GroupBudget_Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NETCore.MailKit.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<GroupBudgetUser>(options => options.SignIn.RequireConfirmedAccount = false)  // voorlopig geen e-mail bevestiging voor de account
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Needed for RESTFull API communication
builder.Services.AddControllers();

builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
builder.Services.Configure<MailKitOptions>
    (
       options => 
        {
            //Option 1:  Rubbish, as this information is hardcoded
            //options.Server = "ServierName";
            //options.Port = Convert.ToInt32("465");
            //options.Account ="MyAccount";
            //options.Password = "Abc!12345";
            //options.SenderEmail = "Admin@GroupBudget.be";
            //options.SenderName = "Administrator";

            // Option 2:  Dangerous, as information is available for anyone having access to appsettings.json
            //options.Server = builder.Configuration["ExternalProviders:MailKit:SMTP:Address"];
            //options.Port = Convert.ToInt32(builder.Configuration["ExternalProviders:MailKit:SMTP:Port"]);
            //options.Account = builder.Configuration["ExternalProviders:MailKit:SMTP:Account"];
            //options.Password = builder.Configuration["ExternalProviders:MailKit:SMTP:Password"];
            //options.SenderEmail = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
            //options.SenderName = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderName"];
        }
    );

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)   // use languate identification as suffix
    .AddDataAnnotationsLocalization();          // provide automatic translation of [Display] dataannotation

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GroupBudget_Web", Version = "v1" });
});

builder.Services.AddTransient<IMyUser, MyUser>();



var app = builder.Build();
Globals.App = app;



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
}



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    ApplicationDbContext context = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
    var userManager = services.GetRequiredService<UserManager<GroupBudgetUser>>();  // Needed to assign the first users
    SeedDataContext.Initialize(context, userManager);
}

var supportedCultures = new[] { "en-US", "fr", "nl" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
       .AddSupportedCultures(supportedCultures)
       .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Add my customized middleware
app.UseMiddleware<MyMiddleWare>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
