using api.Configuration;
using api.RealtimeHubs;
using api.Validation;
using Data;
using Data.Repository;
using Data.Validation;
using Domain.Interfaces;
using Domain.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Serviços
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(AutoMapperConfigDTOs), typeof(AutoMapperConfigViewModels));

// Injeções de dependência
builder.Services.AddScoped<IProdutoService, ProdutoValidation>();
builder.Services.AddScoped<IMesaService, MesaValidation>();
builder.Services.AddScoped<IPedidoService, PedidoValidation>();

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IMesaRepository, MesaRepository>();
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
builder.Services.AddScoped<ISaborRepository, SaborRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IAdicionalRepository, AdicionalRepository>();

// CORS para o frontend acessar
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", builder =>
    {
        builder
            .WithOrigins("http://localhost:1420", "http://192.168.5.5:1420") // Frontend local e em rede
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();


var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Clear();
app.Urls.Add($"http://*:{port}");

// Middleware de CORS
app.UseCors("DefaultPolicy");

// Middleware de roteamento
app.UseRouting();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
});

// Endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<PedidoHub>("/pedidoHub");
    endpoints.MapHub<ProdutoHub>("/produtoHub");
    endpoints.MapHub<AdicionalHub>("/adicionalHub");
    endpoints.MapHub<SaborHub>("/saborHub");   
});

// HTTPS opcional
// app.UseHttpsRedirection();

app.Run();
