using GarmentsAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// DATABASE
// ============================================================

var connectionString =
    builder.Configuration.GetConnectionString("GarmentDB");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception(
        "GarmentDB connection string is missing from appsettings.json");
}

// ============================================================
// CONTROLLERS
// ============================================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============================================================
// REPOSITORIES
// ============================================================

builder.Services.AddScoped<EmployeeRepository>(
    provider => new EmployeeRepository(connectionString)
);

builder.Services.AddScoped<InventoryRepository>(
    provider => new InventoryRepository(connectionString)
);

builder.Services.AddScoped<VendorRepository>(
    provider => new VendorRepository(connectionString)
);

builder.Services.AddScoped<CustomerRepository>(
    provider => new CustomerRepository(connectionString)
);

builder.Services.AddScoped<PurchaseRepository>(
    provider => new PurchaseRepository(connectionString)
);

builder.Services.AddScoped<SalesRepository>(
    provider => new SalesRepository(connectionString)
);

builder.Services.AddScoped<UserRepository>(
    provider => new UserRepository(connectionString)
);

// ============================================================
// JWT AUTHENTICATION
// ============================================================

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new Exception(
        "JWT Key is missing from appsettings.json");
}

var jwtIssuer = builder.Configuration["Jwt:Issuer"];

var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtIssuer))
{
    throw new Exception(
        "JWT Issuer is missing from appsettings.json");
}

if (string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new Exception(
        "JWT Audience is missing from appsettings.json");
}

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                // Token validation
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                // Expected issuer
                ValidIssuer = jwtIssuer,

                // Expected audience
                ValidAudience = jwtAudience,

                // Signing key
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    ),

                // IMPORTANT FOR [Authorize(Roles = "...")]
                RoleClaimType = ClaimTypes.Role,

                // IMPORTANT FOR User.Identity.Name
                NameClaimType = ClaimTypes.Name,

                // Don't allow expired tokens
                ClockSkew = TimeSpan.Zero
            };
    });

// ============================================================
// AUTHORIZATION
// ============================================================

builder.Services.AddAuthorization();

// ============================================================
// BUILD APPLICATION
// ============================================================

var app = builder.Build();

// ============================================================
// SWAGGER
// ============================================================

app.UseSwagger();
app.UseSwaggerUI();

// ============================================================
// AUTHENTICATION / AUTHORIZATION
// ============================================================

app.UseAuthentication();

app.UseAuthorization();

// ============================================================
// CONTROLLERS
// ============================================================

app.MapControllers();

app.MapGet("/", () => "Garments API is running");

app.Run();