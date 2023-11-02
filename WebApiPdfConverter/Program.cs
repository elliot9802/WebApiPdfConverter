using Services;

var builder = WebApplication.CreateBuilder(args);

var syncfusionLicenseKey = builder.Configuration["SyncfusionLicenseKey"];
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicenseKey);

// Add services to the container.
builder.Services.AddLogging();
builder.Services.AddControllers();
//builder.Services.AddMemoryCache();
builder.Services.AddTransient<IPdfConverterUtility, SyncfusionConvertService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IPdfConversionService, PdfConversionService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", builder =>
    {

#if DEBUG
        builder.WithOrigins("http://localhost:5173")
#endif
#if RELEASE
        builder.WithOrigins("https://*.actorsmartbook.se", "https://*.actorsmartbook.no") 
#endif
               .SetIsOriginAllowedToAllowWildcardSubdomains()
               .WithMethods("POST")
               .WithHeaders("Content-Type");
    });
});
/* 
CORS allows specific external origins access to your API via browsers. 
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Use CORS middleware
app.UseCors("DefaultCorsPolicy");

app.MapControllers();

app.Run();
