using ExpenseControlAPI.DTOs;
using ExpenseControlAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControlAPI.Controllers;

/// <summary>
/// Controller que gerencia os endpoints relacionados a categorias.
/// Fornece endpoints para criar e listar categorias.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// GET: api/categories
    /// Lista todas as categorias cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de categorias com status 200 OK</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CategoryResponse>>>> GetAll()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(new ApiResponse<List<CategoryResponse>>
            {
                Success = true,
                Message = "Categories retrieved successfully",
                Data = categories
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<CategoryResponse>>
            {
                Success = false,
                Message = $"Error retrieving categories: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }

    /// <summary>
    /// GET: api/categories/{id}
    /// Recupera uma categoria específica pelo seu ID.
    /// </summary>
    /// <param name="id">ID da categoria a recuperar</param>
    /// <returns>Dados da categoria ou 404 se não encontrada</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CategoryResponse>>> GetById(Guid id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new ApiResponse<CategoryResponse>
                {
                    Success = false,
                    Message = "Category not found",
                    ErrorCode = "CATEGORY_NOT_FOUND"
                });
            }

            return Ok(new ApiResponse<CategoryResponse>
            {
                Success = true,
                Message = "Category retrieved successfully",
                Data = category
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<CategoryResponse>
            {
                Success = false,
                Message = $"Error retrieving category: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }

    /// <summary>
    /// POST: api/categories
    /// Cria uma nova categoria no sistema.
    /// A descrição deve ser única.
    /// </summary>
    /// <param name="request">Dados da categoria a criar (descrição e finalidade)</param>
    /// <returns>Dados da categoria criada com status 201 Created</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoryResponse>>> Create([FromBody] CreateCategoryRequest request)
    {
        try
        {
            var category = await _categoryService.CreateCategoryAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, new ApiResponse<CategoryResponse>
            {
                Success = true,
                Message = "Category created successfully",
                Data = category
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<CategoryResponse>
            {
                Success = false,
                Message = ex.Message,
                ErrorCode = "VALIDATION_ERROR"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<CategoryResponse>
            {
                Success = false,
                Message = $"Error creating category: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }
}
