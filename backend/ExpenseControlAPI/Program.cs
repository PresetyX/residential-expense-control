using ExpenseControlAPI.Data;
using ExpenseControlAPI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ===== CONFIGURAÇÃO DE LOGGING =====
// Configura o Serilog para logs estruturados
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console();
});

// ===== CONFIGURAÇÃO DE BANCO DE DADOS =====
// Registra o DbContext com Entity Framework Core e SQLite
// O banco de dados será criado automaticamente no arquivo "expense_control.db"
builder.Services.AddDbContext<ExpenseControlContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ??
        "Data Source=expense_control.db");
});

// ===== REGISTR O DE DEPENDÃNCIAS (Dependency Injection) =====
// Registra as interfaces e suas implementações no container de DI
// Isso permite que os controladores injetem os serviços automaticamente
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IReportService, ReportService>();

// ===== CONFIGURAÇÃO DE CONTROLADORES E SWAGGER =====
// Adiciona suporte a controladores
builder.Services.AddControllers();

// Habilita CORS para que o front-end possa fazer requisições
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Habilita o Swagger/OpenAPI para documentação da API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Expense Control API",
        Version = "v1",
        Description = "API para controle de gastos residenciais"
    });
});

var app = builder.Build();

// ===== EXECUTAÇÃO DE MIGRATIONS =====
// Cria automaticamente as tabelas no banco de dados se não existirem
// Isso usa as configurações do DbContext para criar o schema
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExpenseControlContext>();
    try
    {
        // Aplica todas as migrações pendentes
        // Se o banco não existir, ele será criado com todas as tabelas
        await db.Database.EnsureCreatedAsync();
        Console.WriteLine("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database migration: {ex.Message}");
    }
}

// ===== MIDDLEWARE PIPELINE =====
// Configura o pipeline HTTP de requisições

if (app.Environment.IsDevelopment())
{
    // Em desenvolvimento, habilita Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS deve estar antes de Authorization
app.UseCors("AllowAll");

// Redireciona HTTP para HTTPS (opcional)
app.UseHttpsRedirection();

// Habilita autorização se necessário
app.UseAuthorization();

// Mapeia os controladores aos endpoints
app.MapControllers();

// Inicia a aplicação
app.Run();
