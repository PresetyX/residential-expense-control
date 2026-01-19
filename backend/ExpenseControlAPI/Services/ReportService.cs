using ExpenseControlAPI.Data;
using ExpenseControlAPI.DTOs;
using ExpenseControlAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControlAPI.Services;

/// <summary>
/// Serviço que encapsula a lógica para geração de relatórios.
/// Fornece totalizações por pessoa e por categoria.
/// </summary>
public interface IReportService
{
    Task<PersonTotalReportResponse> GetPersonTotalsAsync();
    Task<CategoryTotalReportResponse> GetCategoryTotalsAsync();
}

public class ReportService : IReportService
{
    private readonly ExpenseControlContext _context;

    public ReportService(ExpenseControlContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gera relatório com totalizações por pessoa.
    /// Para cada pessoa, calcula:
    /// - Total de receitas (soma de todas as transações do tipo Income)
    /// - Total de despesas (soma de todas as transações do tipo Expense)
    /// - Saldo (receitas - despesas)
    /// 
    /// Além disso, calcula os totais gerais de todas as pessoas.
    /// </summary>
    /// <returns>Relatório com totais por pessoa e totais gerais</returns>
    public async Task<PersonTotalReportResponse> GetPersonTotalsAsync()
    {
        // Busca todas as pessoas e suas transações
        var people = await _context.People
            .Include(p => p.Transactions)
            .AsNoTracking()
            .ToListAsync();

        var personTotals = new List<PersonTotalResponse>();
        decimal grandTotalIncome = 0;
        decimal grandTotalExpense = 0;

        // Para cada pessoa, calcula seus totais
        foreach (var person in people)
        {
            // Soma de todas as receitas (transacções do tipo Income)
            var totalIncome = person.Transactions?
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount) ?? 0;

            // Soma de todas as despesas (transações do tipo Expense)
            var totalExpense = person.Transactions?
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount) ?? 0;

            // Acumula os totais gerais
            grandTotalIncome += totalIncome;
            grandTotalExpense += totalExpense;

            // Cria objeto de resposta para esta pessoa
            personTotals.Add(new PersonTotalResponse
            {
                PersonId = person.Id,
                PersonName = person.Name,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense
            });
        }

        // Retorna o relatório completo com a lista de pessoas e os totais gerais
        return new PersonTotalReportResponse
        {
            People = personTotals,
            GrandTotalIncome = grandTotalIncome,
            GrandTotalExpense = grandTotalExpense
        };
    }

    /// <summary>
    /// Gera relatório com totalizações por categoria.
    /// Para cada categoria, calcula:
    /// - Total de receitas (soma de todas as transações do tipo Income)
    /// - Total de despesas (soma de todas as transações do tipo Expense)
    /// - Saldo (receitas - despesas)
    /// 
    /// Além disso, calcula os totais gerais de todas as categorias.
    /// </summary>
    /// <returns>Relatório com totais por categoria e totais gerais</returns>
    public async Task<CategoryTotalReportResponse> GetCategoryTotalsAsync()
    {
        // Busca todas as categorias e suas transações
        var categories = await _context.Categories
            .Include(c => c.Transactions)
            .AsNoTracking()
            .ToListAsync();

        var categoryTotals = new List<CategoryTotalResponse>();
        decimal grandTotalIncome = 0;
        decimal grandTotalExpense = 0;

        // Para cada categoria, calcula seus totais
        foreach (var category in categories)
        {
            // Soma de todas as receitas (transações do tipo Income)
            var totalIncome = category.Transactions?
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount) ?? 0;

            // Soma de todas as despesas (transações do tipo Expense)
            var totalExpense = category.Transactions?
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount) ?? 0;

            // Acumula os totais gerais
            grandTotalIncome += totalIncome;
            grandTotalExpense += totalExpense;

            // Cria objeto de resposta para esta categoria
            categoryTotals.Add(new CategoryTotalResponse
            {
                CategoryId = category.Id,
                CategoryDescription = category.Description,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense
            });
        }

        // Retorna o relatório completo com a lista de categorias e os totais gerais
        return new CategoryTotalReportResponse
        {
            Categories = categoryTotals,
            GrandTotalIncome = grandTotalIncome,
            GrandTotalExpense = grandTotalExpense
        };
    }
}
