using ExpenseControlAPI.Models;

namespace ExpenseControlAPI.DTOs;

/// <summary>
/// DTO para requisição de criação de uma nova transação.
/// Contém os dados necessários para registrar uma transação no sistema.
/// </summary>
public class CreateTransactionRequest
{
    /// <summary>
    /// Descrição da transação (ex: "Compras supermercado", "Salário").
    /// Campo obrigatório.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Valor da transação em reais. Deve ser um valor positivo.
    /// Campo obrigatório.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Tipo da transação: Expense (despesa) ou Income (receita).
    /// Campo obrigatório.
    /// </summary>
    public TransactionType Type { get; set; } = TransactionType.Expense;

    /// <summary>
    /// ID da pessoa que está realizando a transação.
    /// Campo obrigatório.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// ID da categoria da transação.
    /// Campo obrigatório. A categoria deve ser válida para o tipo de transação.
    /// </summary>
    public Guid CategoryId { get; set; }
}

/// <summary>
/// DTO para resposta com dados de uma transação.
/// É retornado pelo servidor em listas ou após criação.
/// </summary>
public class TransactionResponse
{
    /// <summary>Identificador único da transação</summary>
    public Guid Id { get; set; }

    /// <summary>Descrição da transação</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Valor da transação</summary>
    public decimal Amount { get; set; }

    /// <summary>Tipo da transação (Despesa ou Receita)</summary>
    public TransactionType Type { get; set; }

    /// <summary>Data de criação da transação</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>ID da pessoa</summary>
    public Guid PersonId { get; set; }

    /// <summary>Nome da pessoa</summary>
    public string PersonName { get; set; } = string.Empty;

    /// <summary>ID da categoria</summary>
    public Guid CategoryId { get; set; }

    /// <summary>Descrição da categoria</summary>
    public string CategoryDescription { get; set; } = string.Empty;
}
