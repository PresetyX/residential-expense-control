namespace ExpenseControlAPI.DTOs;

/// <summary>
/// DTO que encapsula a resposta padrão de qualquer endpoint da API.
/// Fornece um formato consistente com sucesso/erro, dados e mensagem.
/// </summary>
/// <typeparam name="T">Tipo dos dados retornados na resposta</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indica se a requisição foi bem-sucedida.
    /// true = sucesso, false = erro
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Mensagem descritiva da resposta (ex: "Pessoa criada com sucesso" ou "ID de pessoa inválido")
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Dados retornados pela API. Pode ser null em caso de erro.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Código de erro específico para identificação programada de problemas.
    /// Null se a requisição foi bem-sucedida.
    /// </summary>
    public string? ErrorCode { get; set; }
}
