using ConsultaDeCreditos.API.HealthChecks;
using ConsultaDeCreditos.API.Middlewares;
using ConsultaDeCreditos.IoC;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Consulta de Créditos Constituídos",
        Version = "v1",
        Description = "API RESTful para consulta e integração de créditos constituídos com mensageria e background processing",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Equipe de Desenvolvimento",
            Email = "dev@example.com"
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AdicionarInfrastrutura(builder.Configuration);
builder.Services.AdicionarApplication();

builder.Services.AddHealthChecks()
    .AddCheck<SelfHealthCheck>("self", tags: ["self"])
    .AddCheck<ReadyHealthCheck>("ready", tags: ["ready"]);

// CORS para ambientes de desenvolvimento
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configurar forwarded headers para funcionar atrás de proxy/load balancer (Render, Azure, AWS, etc.)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Usar forwarded headers ANTES de outros middlewares
app.UseForwardedHeaders();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// Swagger disponível em todos os ambientes para facilitar testes
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Créditos v1");
    c.RoutePrefix = string.Empty;
});

app.UseCors("AllowAll");

// HTTPS Redirection apenas em Development (em produção o proxy já faz isso)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// Health Check Endpoints
app.MapHealthChecks("/self", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("self")
});

app.MapHealthChecks("/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.Logger.LogInformation("Aplicação iniciada com sucesso");

app.Run();
