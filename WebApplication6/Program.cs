using GenericRepositoryApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL baðlantýsýný ekleyin
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS ayarlarýný ekliyoruz
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// DI için servisleri ekleyin
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Unit of Work ekleniyor
builder.Services.AddScoped<PointService>(); // PointService ekleniyor

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Veritabaný baðlantýsýný kontrol etmek için try-catch bloðu ekleyin
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        // Veritabaný baðlantýsýný kontrol etmek için basit bir sorgu çalýþtýrýyoruz
        await dbContext.Database.OpenConnectionAsync();
        await dbContext.Database.CloseConnectionAsync();
        Console.WriteLine("Veritabanýna baðlantý baþarýlý.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Veritabanýna baðlantý baþarýsýz: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS'u kullanýyoruz
app.UseCors("AllowAllOrigins");

app.UseAuthorization();
app.MapControllers();
app.Run();
