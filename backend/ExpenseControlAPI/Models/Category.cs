namespace ExpenseControlAPI.Models;

/// <summary>
/// Enumerador que define a finalidade de uma categoria.
/// Cada categoria pode ser utilizada para Despesa, Receita ou ambas.
/// </summary>
public enum CategoryPurpose
{
    /// <summary>Categoria pode ser usada apenas para despesas</summary>
    Expense = 0,
    
    /// <summary>Categoria pode ser usada apenas para receitas</summary>
    Income = 1,
    
    /// <summary>Categoria pode ser usada para despesas e receitas</summary>
    Both = 2
}

/// <summary>
/// Representa uma categoria de transação no sistema.
/// Categorias são usadas para classificar receitas e despesas.
/// </summary>
public class Category
{
    /// <summary>
    /// Identificador único da categoria (UUID/GUID gerado automaticamente).
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Descrição da categoria (ex: "Alimentação", "Transporte", "Salário").
    /// Campo obrigatório e único.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Define a finalidade da categoria: Despesa, Receita ou Ambas.
    /// Usado para validar se uma transação pode usar esta categoria.
    /// </summary>
    public CategoryPurpose Purpose { get; set; } = CategoryPurpose.Expense;

    /// <summary>
    /// Relacionamento: uma categoria pode ter múltiplas transações.
    /// </summary>
    public ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
}
