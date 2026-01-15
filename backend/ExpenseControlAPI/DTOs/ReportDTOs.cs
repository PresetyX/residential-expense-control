namespace ExpenseControlAPI.DTOs;

/// <summary>
/// DTO que representa os totais de uma pessoa (receitas, despesas e saldo).
/// Usado no relatório de totais por pessoa.
/// </summary>
public class PersonTotalResponse
{
    /// <summary>ID da pessoa</summary>
    public Guid PersonId { get; set; }

    /// <summary>Nome da pessoa</summary>
    public string PersonName { get; set; } = string.Empty;

    /// <summary>Total de receitas da pessoa</summary>
    public decimal TotalIncome { get; set; }

    /// <summary>Total de despesas da pessoa</summary>
    public decimal TotalExpense { get; set; }

    /// <summary>Saldo: TotalIncome - TotalExpense</summary>
    public decimal Balance => TotalIncome - TotalExpense;
}

/// <summary>
/// DTO que representa os totais por pessoa incluindo um resumo geral.
/// Retornado pelo endpoint de relatório de totais por pessoa.
/// </summary>
public class PersonTotalReportResponse
{
    /// <summary>Lista de todas as pessoas com seus totais</summary>
    public List<PersonTotalResponse> People { get; set; } = new();

    /// <summary>Total geral de todas as receitas</summary>
    public decimal GrandTotalIncome { get; set; }

    /// <summary>Total geral de todas as despesas</summary>
    public decimal GrandTotalExpense { get; set; }

    /// <summary>Saldo geral: GrandTotalIncome - GrandTotalExpense</summary>
    public decimal GrandTotalBalance => GrandTotalIncome - GrandTotalExpense;
}

/// <summary>
/// DTO que representa os totais de uma categoria (receitas, despesas e saldo).
/// Usado no relatório de totais por categoria.
/// </summary>
public class CategoryTotalResponse
{
    /// <summary>ID da categoria</summary>
    public Guid CategoryId { get; set; }

    /// <summary>Descrição da categoria</summary>
    public string CategoryDescription { get; set; } = string.Empty;

    /// <summary>Total de receitas nesta categoria</summary>
    public decimal TotalIncome { get; set; }

    /// <summary>Total de despesas nesta categoria</summary>
    public decimal TotalExpense { get; set; }

    /// <summary>Saldo: TotalIncome - TotalExpense</summary>
    public decimal Balance => TotalIncome - TotalExpense;
}

/// <summary>
/// DTO que representa os totais por categoria incluindo um resumo geral.
/// Retornado pelo endpoint de relatório de totais por categoria.
/// </summary>
public class CategoryTotalReportResponse
{
    /// <summary>Lista de todas as categorias com seus totais</summary>
    public List<CategoryTotalResponse> Categories { get; set; } = new();

    /// <summary>Total geral de todas as receitas</summary>
    public decimal GrandTotalIncome { get; set; }

    /// <summary>Total geral de todas as despesas</summary>
    public decimal GrandTotalExpense { get; set; }

    /// <summary>Saldo geral: GrandTotalIncome - GrandTotalExpense</summary>
    public decimal GrandTotalBalance => GrandTotalIncome - GrandTotalExpense;
}
