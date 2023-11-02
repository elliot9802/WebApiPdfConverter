using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddTransient<IPdfConverterUtility, SyncfusionConvertService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IPdfConversionService, PdfConversionService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#if DEBUG
// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueAppPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5173", "https://*.actorsmartbook.se", "https://*.actorsmartbook.no") 
               .SetIsOriginAllowedToAllowWildcardSubdomains()
               .WithMethods("POST")
               .WithHeaders("Content-Type");
    });
});
/* 
CORS allows specific external origins access to your API via browsers. 
*/
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

#if DEBUG
// Use CORS middleware
app.UseCors("VueAppPolicy");
#endif

app.MapControllers();

app.Run();
