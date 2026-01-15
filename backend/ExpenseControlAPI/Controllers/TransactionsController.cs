using ExpenseControlAPI.DTOs;
using ExpenseControlAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControlAPI.Controllers;

/// <summary>
/// Controller que gerencia os endpoints relacionados a transações.
/// Fornece endpoints para criar e listar transações.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// GET: api/transactions
    /// Lista todas as transações cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de transações com status 200 OK</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<TransactionResponse>>>> GetAll()
    {
        try
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(new ApiResponse<List<TransactionResponse>>
            {
                Success = true,
                Message = "Transactions retrieved successfully",
                Data = transactions
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<TransactionResponse>>
            {
                Success = false,
                Message = $"Error retrieving transactions: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }

    /// <summary>
    /// GET: api/transactions/by-person/{personId}
    /// Lista todas as transações de uma pessoa específica.
    /// </summary>
    /// <param name="personId">ID da pessoa</param>
    /// <returns>Lista de transações da pessoa com status 200 OK</returns>
    [HttpGet("by-person/{personId:guid}")]
    public async Task<ActionResult<ApiResponse<List<TransactionResponse>>>> GetByPerson(Guid personId)
    {
        try
        {
            var transactions = await _transactionService.GetTransactionsByPersonAsync(personId);
            return Ok(new ApiResponse<List<TransactionResponse>>
            {
                Success = true,
                Message = "Transactions retrieved successfully",
                Data = transactions
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<TransactionResponse>>
            {
                Success = false,
                Message = $"Error retrieving transactions: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }

    /// <summary>
    /// POST: api/transactions
    /// Cria uma nova transação no sistema.
    /// Aplica todas as validações de regras de negócio:
    /// - Menores de idade só podem ter despesas
    /// - Categoria deve corresponder ao tipo de transação
    /// - Pessoa e categoria devem existir
    /// - Valor deve ser positivo
    /// </summary>
    /// <param name="request">Dados da transação a criar</param>
    /// <returns>Dados da transação criada com status 201 Created</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TransactionResponse>>> Create([FromBody] CreateTransactionRequest request)
    {
        try
        {
            var transaction = await _transactionService.CreateTransactionAsync(request);
            return CreatedAtAction(nameof(GetByPerson), new { personId = transaction.PersonId }, new ApiResponse<TransactionResponse>
            {
                Success = true,
                Message = "Transaction created successfully",
                Data = transaction
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<TransactionResponse>
            {
                Success = false,
                Message = ex.Message,
                ErrorCode = "VALIDATION_ERROR"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<TransactionResponse>
            {
                Success = false,
                Message = $"Error creating transaction: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }
}
