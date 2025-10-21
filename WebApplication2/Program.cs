using ClickHouse.Client;
using ClickHouse.Client.ADO;
<<<<<<< HEAD
using WebApplication2.Options;
=======
>>>>>>> d8755cb6b101e3725d3ccc7528080c79e3058c94
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
<<<<<<< HEAD
builder.Services.Configure<ClickHouseOptions>(builder.Configuration.GetSection(ClickHouseOptions.ClickHouseSettings));
=======
>>>>>>> d8755cb6b101e3725d3ccc7528080c79e3058c94

//builder.Services.AddSingleton<IClickHouseConnection, ClickHouseConnection>();
builder.Services.AddSingleton<IClickHouseService, ClickHouseService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ClickHouse}/{action=Test}/");


app.Run();
