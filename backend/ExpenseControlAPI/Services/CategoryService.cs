using ExpenseControlAPI.Data;
using ExpenseControlAPI.DTOs;
using ExpenseControlAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControlAPI.Services;

/// <summary>
/// Serviço que encapsula a lógica de negócio relacionada a categorias.
/// Gerencia criação e leitura de categorias.
/// </summary>
public interface ICategoryService
{
    Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request);
    Task<List<CategoryResponse>> GetAllCategoriesAsync();
    Task<CategoryResponse?> GetCategoryByIdAsync(Guid id);
}

public class CategoryService : ICategoryService
{
    private readonly ExpenseControlContext _context;

    public CategoryService(ExpenseControlContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Cria uma nova categoria no sistema.
    /// Valida que a descrição não está vazia e é única.
    /// </summary>
    /// <param name="request">Dados da categoria a ser criada</param>
    /// <returns>Resposta com os dados da categoria criada</returns>
    /// <exception cref="ArgumentException">Lançado se a descrição for inválida ou duplicada</exception>
    public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request)
    {
        // Validação: descrição não pode estar vazia
        if (string.IsNullOrWhiteSpace(request.Description))
            throw new ArgumentException("Description cannot be empty.");

        // Validação: descrição deve ser única
        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.Description == request.Description.Trim());

        if (existingCategory != null)
            throw new ArgumentException("Category with this description already exists.");

        // Cria nova categoria com ID gerado automaticamente
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Description = request.Description.Trim(),
            Purpose = request.Purpose
        };

        // Adiciona à base de dados
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return MapToResponse(category);
    }

    /// <summary>
    /// Recupera todas as categorias cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as categorias</returns>
    public async Task<List<CategoryResponse>> GetAllCategoriesAsync()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .ToListAsync();

        return categories.Select(MapToResponse).ToList();
    }

    /// <summary>
    /// Recupera uma categoria específica pelo seu ID.
    /// </summary>
    /// <param name="id">ID da categoria a buscar</param>
    /// <returns>Dados da categoria ou null se não encontrada</returns>
    public async Task<CategoryResponse?> GetCategoryByIdAsync(Guid id)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return category != null ? MapToResponse(category) : null;
    }

    /// <summary>
    /// Converte uma entidade Category em um DTO CategoryResponse.
    /// </summary>
    private static CategoryResponse MapToResponse(Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Description = category.Description,
            Purpose = category.Purpose
        };
    }
}
