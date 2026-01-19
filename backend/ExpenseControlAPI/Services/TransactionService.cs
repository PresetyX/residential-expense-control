using ExpenseControlAPI.Data;
using ExpenseControlAPI.DTOs;
using ExpenseControlAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControlAPI.Services;

/// <summary>
/// Serviço que encapsula a lógica de negócio relacionada a transações.
/// Gerencia criação e leitura de transações com validações de regras de negócio.
/// </summary>
public interface ITransactionService
{
    Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request);
    Task<List<TransactionResponse>> GetAllTransactionsAsync();
    Task<List<TransactionResponse>> GetTransactionsByPersonAsync(Guid personId);
}

public class TransactionService : ITransactionService
{
    private readonly ExpenseControlContext _context;
    private const int ADULT_AGE = 18; // Constante para definír a idade mínima para receitas

    public TransactionService(ExpenseControlContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Cria uma nova transação no sistema.
    /// Aplica todas as regras de negócio:
    /// 1. Valida se o valor é positivo
    /// 2. Valida se a pessoa existe
    /// 3. Valida se a categoria existe
    /// 4. Valida se menores de idade não tentam criar receitas
    /// 5. Valida se a categoria é compatível com o tipo de transação
    /// </summary>
    /// <param name="request">Dados da transação a ser criada</param>
    /// <returns>Resposta com os dados da transação criada</returns>
    /// <exception cref="ArgumentException">Lançado se qualquer validação falhar</exception>
    public async Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request)
    {
        // VALIDAÇÃO 1: Valor deve ser positivo
        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be a positive value.");

        // VALIDAÇÃO 2: Descrição não pode estar vazia
        if (string.IsNullOrWhiteSpace(request.Description))
            throw new ArgumentException("Description cannot be empty.");

        // VALIDAÇÃO 3: Pessoa deve existir
        var person = await _context.People.FindAsync(request.PersonId);
        if (person == null)
            throw new ArgumentException("Person not found.");

        // VALIDAÇÃO 4: REGRA DE NEGÓCIO - Menores de idade só podem ter despesas
        // Se a pessoa tem menos de 18 anos e está tentando criar uma receita, rejeitamos
        if (person.Age < ADULT_AGE && request.Type == TransactionType.Income)
        {
            throw new ArgumentException(
                $"Person under {ADULT_AGE} years old can only register expenses, not income.");
        }

        // VALIDAÇÃO 5: Categoria deve existir
        var category = await _context.Categories.FindAsync(request.CategoryId);
        if (category == null)
            throw new ArgumentException("Category not found.");

        // VALIDAÇÃO 6: REGRA DE NEGÓCIO - Categoria deve ser compatível com o tipo de transação
        // Se o tipo da transação for Expense, a categoria deve aceitar despesas
        if (request.Type == TransactionType.Expense)
        {
            if (category.Purpose != CategoryPurpose.Expense && category.Purpose != CategoryPurpose.Both)
            {
                throw new ArgumentException(
                    "Selected category is not valid for expense transactions.");
            }
        }
        // Se o tipo da transação for Income, a categoria deve aceitar receitas
        else if (request.Type == TransactionType.Income)
        {
            if (category.Purpose != CategoryPurpose.Income && category.Purpose != CategoryPurpose.Both)
            {
                throw new ArgumentException(
                    "Selected category is not valid for income transactions.");
            }
        }

        // Todas as validações passaram, cria a transação
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = request.Description.Trim(),
            Amount = request.Amount,
            Type = request.Type,
            PersonId = request.PersonId,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        // Adiciona à base de dados
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return await MapToResponseAsync(transaction, person, category);
    }

    /// <summary>
    /// Recupera todas as transações cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as transações</returns>
    public async Task<List<TransactionResponse>> GetAllTransactionsAsync()
    {
        var transactions = await _context.Transactions
            .Include(t => t.Person)
            .Include(t => t.Category)
            .AsNoTracking()
            .ToListAsync();

        return transactions
            .Select(t => MapToResponse(t))
            .ToList();
    }

    /// <summary>
    /// Recupera todas as transações de uma pessoa específica.
    /// </summary>
    /// <param name="personId">ID da pessoa</param>
    /// <returns>Lista de transações da pessoa</returns>
    public async Task<List<TransactionResponse>> GetTransactionsByPersonAsync(Guid personId)
    {
        var transactions = await _context.Transactions
            .Where(t => t.PersonId == personId)
            .Include(t => t.Person)
            .Include(t => t.Category)
            .AsNoTracking()
            .ToListAsync();

        return transactions
            .Select(t => MapToResponse(t))
            .ToList();
    }

    /// <summary>
    /// Converte uma entidade Transaction em um DTO TransactionResponse.
    /// </summary>
    private static TransactionResponse MapToResponse(Transaction transaction)
    {
        return new TransactionResponse
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Type = transaction.Type,
            CreatedAt = transaction.CreatedAt,
            PersonId = transaction.PersonId,
            PersonName = transaction.Person?.Name ?? string.Empty,
            CategoryId = transaction.CategoryId,
            CategoryDescription = transaction.Category?.Description ?? string.Empty
        };
    }

    /// <summary>
    /// Versão assíncrona do MapToResponse que busca os dados relacionados.
    /// </summary>
    private static Task<TransactionResponse> MapToResponseAsync(
        Transaction transaction,
        Person person,
        Category category)
    {
        var response = new TransactionResponse
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Type = transaction.Type,
            CreatedAt = transaction.CreatedAt,
            PersonId = transaction.PersonId,
            PersonName = person.Name,
            CategoryId = transaction.CategoryId,
            CategoryDescription = category.Description
        };

        return Task.FromResult(response);
    }
}
