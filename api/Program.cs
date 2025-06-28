using api.Configuration;
using api.Validation;
using Data;
using Data.Repository;
using Data.Validation;
using Domain.Interfaces;
using Domain.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Serviços
builder.Services.AddControllers(); // <-- ESSENCIAL
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(AutoMapperConfigDTOs), typeof(AutoMapperConfigViewModels));

builder.Services.AddScoped<IProdutoService, ProdutoValidation>();
builder.Services.AddScoped<IMesaService, MesaValidation>();
builder.Services.AddScoped<IPedidoService, PedidoValidation>();

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IMesaRepository, MesaRepository>();
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
builder.Services.AddScoped<ISaborRepository, SaborRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IAdicionalRepository, AdicionalRepository>();
// builder.Services.AddScoped<IItemRepository, IItemRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:1420") // substitua pela porta do seu React
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});




var app = builder.Build();
app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
});

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers(); // <-- ESSENCIAL para rotas funcionarem

// app.UseHttpsRedirection();

app.Run();
