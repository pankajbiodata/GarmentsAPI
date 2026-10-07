using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GarmentsAPI;
using System.Configuration;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Garments API is running");
app.Run();
