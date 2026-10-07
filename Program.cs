using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GarmentsAPI;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register EmployeeRepository for DI
builder.Services.AddScoped<EmployeeRepository>(provider =>
    new EmployeeRepository("Server='localhost';Port=3306;Database=garmentsdb;Uid=dev;Pwd=Dev1@;"));
builder.Services.AddScoped<InventoryRepository>(provider =>
    new InventoryRepository("Server='localhost';Port=3306;Database=garmentsdb;Uid=dev;Pwd=Dev1@;"));
builder.Services.AddScoped<VendorRepository>(provider =>
    new VendorRepository("Server='localhost';Port=3306;Database=garmentsdb;Uid=dev;Pwd=Dev1@;"));
builder.Services.AddScoped<CustomerRepository>(provider =>
    new CustomerRepository("Server='localhost';Port=3306;Database=garmentsdb;Uid=dev;Pwd=Dev1@;"));
builder.Services.AddScoped<PurchaseRepository>(provider =>
    new PurchaseRepository("Server='localhost';Port=3306;Database=garmentsdb;Uid=dev;Pwd=Dev1@;"));
builder.Services.AddScoped<SalesRepository>(provider =>
    new SalesRepository("Server='localhost';Port=3306;Database=garmentsdb;Uid=dev;Pwd=Dev1@;"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ============================================================
// JWT AUTHENTICATION
// ============================================================

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new Exception("JWT Key is missing from appsettings.json");
}

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme
    )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    ),

                ClockSkew = TimeSpan.Zero
            };
    });


// ============================================================
// AUTHORIZATION
// ============================================================

builder.Services.AddAuthorization();


// ============================================================
// DATABASE
// ============================================================

var connectionString =
    builder.Configuration.GetConnectionString(
        "GarmentDB"
    );


// ============================================================
// REPOSITORIES
// ============================================================

builder.Services.AddScoped<UserRepository>(
    provider =>
        new UserRepository(connectionString)
);
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
// IMPORTANT:
// Authentication MUST come before Authorization.

app.UseAuthentication();


app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Garments API is running");
app.Run();
