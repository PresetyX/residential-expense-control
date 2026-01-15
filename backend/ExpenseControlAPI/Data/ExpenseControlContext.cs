using ExpenseControlAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControlAPI.Data;

/// <summary>
/// Contexto do Entity Framework Core para o sistema de controle de gastos.
/// Gerencia o mapeamento entre as entidades C# e as tabelas do banco de dados SQLite.
/// </summary>
public class ExpenseControlContext : DbContext
{
    /// <summary>
    /// Construtor que recebe as opções de configuração do DbContext.
    /// </summary>
    public ExpenseControlContext(DbContextOptions<ExpenseControlContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet para gerenciar as pessoas cadastradas no sistema.
    /// </summary>
    public DbSet<Person> People { get; set; } = null!;

    /// <summary>
    /// DbSet para gerenciar as categorias de transações.
    /// </summary>
    public DbSet<Category> Categories { get; set; } = null!;

    /// <summary>
    /// DbSet para gerenciar as transações (receitas e despesas).
    /// </summary>
    public DbSet<Transaction> Transactions { get; set; } = null!;

    /// <summary>
    /// Método chamado quando o modelo está sendo criado.
    /// Configura os mapeamentos, restrições e comportamentos do banco de dados.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Person
        // Define que a chave primária é o ID (GUID)
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Configura os índices
            entity.HasIndex(e => e.Name).IsUnique();

            // Relacionamento: uma pessoa pode ter muitas transações
            // OnDelete(DeleteBehavior.Cascade) = quando uma pessoa é deletada, suas transações são automaticamente removidas
            entity.HasMany(e => e.Transactions)
                .WithOne(t => t.Person)
                .HasForeignKey(t => t.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Descrição deve ser única
            entity.HasIndex(e => e.Description).IsUnique();

            // Relacionamento: uma categoria pode ter muitas transações
            entity.HasMany(e => e.Transactions)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração da entidade Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Define que o Amount deve ser um decimal com 18 casas decimais e 2 casas de precisão
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2);

            // Índices para melhorar performance nas consultas
            entity.HasIndex(e => e.PersonId);
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.CreatedAt);
        });
    }
}
