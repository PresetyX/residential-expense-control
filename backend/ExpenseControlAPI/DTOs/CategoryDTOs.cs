using ExpenseControlAPI.Models;

namespace ExpenseControlAPI.DTOs;

/// <summary>
/// DTO para requisição de criação de uma nova categoria.
/// Contém os dados necessários para registrar uma categoria no sistema.
/// </summary>
public class CreateCategoryRequest
{
    /// <summary>
    /// Descrição da categoria (ex: "Alimentação", "Transporte").
    /// Campo obrigatório.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Finalidade da categoria: Expense, Income ou Both.
    /// Define se a categoria pode ser usada para despesas, receitas ou ambas.
    /// </summary>
    public CategoryPurpose Purpose { get; set; } = CategoryPurpose.Expense;
}

/// <summary>
/// DTO para resposta com dados de uma categoria.
/// É retornado pelo servidor em listas ou após criação.
/// </summary>
public class CategoryResponse
{
    /// <summary>Identificador único da categoria</summary>
    public Guid Id { get; set; }

    /// <summary>Descrição da categoria</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Finalidade da categoria (Despesa, Receita ou Ambas)</summary>
    public CategoryPurpose Purpose { get; set; }
}
