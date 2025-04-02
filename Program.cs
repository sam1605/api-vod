using api_vod;
using api_vod.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();

// ✅ Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()  // Allows requests from any frontend
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// ✅ Swagger Setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Supabase Configuration
builder.Services.Configure<SupabaseConfig>(builder.Configuration.GetSection("Supabase"));

var app = builder.Build();

// ✅ Apply CORS Policy BEFORE Controllers
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
