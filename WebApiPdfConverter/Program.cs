using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<IPdfConverterUtility, SyncfusionConvertService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IPdfConversionService, PdfConversionService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueAppPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5173") 
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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
app.UseCors("VueAppPolicy");

app.MapControllers();

app.Run();
