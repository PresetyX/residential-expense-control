namespace ExpenseControlAPI.Models;

/// <summary>
/// Representa uma pessoa no sistema de controle de gastos.
/// Cada pessoa pode ter múltiplas transações associadas.
/// </summary>
public class Person
{
    /// <summary>
    /// Identificador único da pessoa (UUID/GUID gerado automaticamente).
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Nome da pessoa. Campo obrigatório.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Idade da pessoa em anos. Deve ser um número inteiro positivo.
    /// Pessoas menores de 18 anos só podem ter transações do tipo Despesa.
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Relacionamento: uma pessoa pode ter múltiplas transações.
    /// Quando uma pessoa é deletada, todas as suas transações são removidas em cascata.
    /// </summary>
    public ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
}
