namespace ExpenseControlAPI.DTOs;

/// <summary>
/// DTO para resposta com dados de uma pessoa.
/// É retornado pelo servidor em listas ou após criação.
/// </summary>
public class PersonResponse
{
    /// <summary>Identificador único da pessoa</summary>
    public Guid Id { get; set; }

    /// <summary>Nome da pessoa</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Idade da pessoa em anos</summary>
    public int Age { get; set; }
}
