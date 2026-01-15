namespace ExpenseControlAPI.Models;

/// <summary>
/// Enumerador que define o tipo de transação.
/// Pode ser Despesa (gasto) ou Receita (entrada de dinheiro).
/// </summary>
public enum TransactionType
{
    /// <summary>Transação do tipo despesa (gasto)</summary>
    Expense = 0,
    
    /// <summary>Transação do tipo receita (entrada)</summary>
    Income = 1
}

/// <summary>
/// Representa uma transação (despesa ou receita) no sistema.
/// Toda transação deve estar associada a uma pessoa e uma categoria.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Identificador único da transação (UUID/GUID gerado automaticamente).
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Descrição da transação.
    /// Exemplo: "Compras no supermercado", "Salário do mês", etc.
    /// Campo obrigatório.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Valor da transação em formato decimal (reais).
    /// Sempre deve ser um valor positivo.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Tipo da transação: Expense (despesa) ou Income (receita).
    /// </summary>
    public TransactionType Type { get; set; } = TransactionType.Expense;

    /// <summary>
    /// Data em que a transação foi registrada.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Chaves Estrangeiras

    /// <summary>
    /// Identificador da pessoa que realizou a transação.
    /// Propriedade de chave estrangeira para a entidade Person.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Referência à pessoa que realizou a transação.
    /// Navegação para a entidade Person.
    /// </summary>
    public Person? Person { get; set; }

    /// <summary>
    /// Identificador da categoria da transação.
    /// Propriedade de chave estrangeira para a entidade Category.
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Referência à categoria da transação.
    /// Navegação para a entidade Category.
    /// </summary>
    public Category? Category { get; set; }
}
