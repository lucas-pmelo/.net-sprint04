using Microsoft.EntityFrameworkCore;
using Sprint03.adapter.output.database;
using Sprint03.adapter.input;
using Sprint03.domain.useCase;
using Sprint03.domain.repository;
using FluentValidation;
using Sprint03.adapter.input.dto;
using Sprint03.domain.model;
using Sprint03.infra.service.dto;
using Sprint03.domain.useCase.dto;
using Sprint03.infra.service;
using Sprint03.infra.validator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DotNetEnv.Env.Load();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseOracle(
        "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521)))\n(CONNECT_DATA=(SERVER=DEDICATED)(SID=ORCL)));User Id=RM97707;Password=220600;");
});

builder.Services.AddControllers();

// CUSTOMER
builder.Services.AddScoped<ICustomerAdapter, CustomerAdapter>();
builder.Services.AddScoped<ICustomerUseCase, CustomerUseCase>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IValidator<Customer>, CustomerValidator>();

builder.Services.AddHttpClient<ICepValidationService, CepValidationService>();
builder.Services.AddScoped<IAiService, AiService>();

// AGREEMENT
builder.Services.AddScoped<IAgreementAdapter, AgreementAdapter>();
builder.Services.AddScoped<IAgreementUseCase, AgreementUseCase>();
builder.Services.AddScoped<IAgreementRepository, AgreementRepository>();

builder.Services.AddScoped<IValidator<Agreement>, AgreementValidator>();

// UNIT
builder.Services.AddScoped<IUnitAdapter, UnitAdapter>();
builder.Services.AddScoped<IUnitUseCase, UnitUseCase>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();

builder.Services.AddScoped<IValidator<Unit>, UnitValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
