namespace ExpenseControlAPI.DTOs;

/// <summary>
/// DTO para requisição de criação de uma nova pessoa.
/// Contém os dados necessários para registrar uma pessoa no sistema.
/// </summary>
public class CreatePersonRequest
{
    /// <summary>
    /// Nome da pessoa. Campo obrigatório.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Idade da pessoa em anos. Deve ser um número inteiro positivo.
    /// Campo obrigatório.
    /// </summary>
    public int Age { get; set; }
}
