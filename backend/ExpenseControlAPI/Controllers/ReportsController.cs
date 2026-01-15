using ExpenseControlAPI.DTOs;
using ExpenseControlAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControlAPI.Controllers;

/// <summary>
/// Controller que gerencia os endpoints de relatórios.
/// Fornece endpoints para obter totalizações por pessoa e por categoria.
/// </summary>
[ApiController]
[Route("api/relatorios")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// GET: api/relatorios/por-pessoa
    /// Gera relatório com totalizações por pessoa.
    /// Para cada pessoa, exibe:
    /// - Total de receitas
    /// - Total de despesas
    /// - Saldo (receitas - despesas)
    /// Além disso, exibe os totais gerais.
    /// </summary>
    /// <returns>Relatório com totais por pessoa e totais gerais</returns>
    [HttpGet("por-pessoa")]
    public async Task<ActionResult<ApiResponse<PersonTotalReportResponse>>> GetPersonTotals()
    {
        try
        {
            var report = await _reportService.GetPersonTotalsAsync();
            return Ok(new ApiResponse<PersonTotalReportResponse>
            {
                Success = true,
                Message = "Person totals report retrieved successfully",
                Data = report
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<PersonTotalReportResponse>
            {
                Success = false,
                Message = $"Error retrieving person totals report: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }

    /// <summary>
    /// GET: api/relatorios/por-categoria
    /// Gera relatório com totalizações por categoria.
    /// Para cada categoria, exibe:
    /// - Total de receitas
    /// - Total de despesas
    /// - Saldo (receitas - despesas)
    /// Além disso, exibe os totais gerais.
    /// </summary>
    /// <returns>Relatório com totais por categoria e totais gerais</returns>
    [HttpGet("por-categoria")]
    public async Task<ActionResult<ApiResponse<CategoryTotalReportResponse>>> GetCategoryTotals()
    {
        try
        {
            var report = await _reportService.GetCategoryTotalsAsync();
            return Ok(new ApiResponse<CategoryTotalReportResponse>
            {
                Success = true,
                Message = "Category totals report retrieved successfully",
                Data = report
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<CategoryTotalReportResponse>
            {
                Success = false,
                Message = $"Error retrieving category totals report: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }
}
