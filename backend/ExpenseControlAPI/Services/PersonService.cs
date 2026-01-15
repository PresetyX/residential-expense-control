using ExpenseControlAPI.Data;
using ExpenseControlAPI.DTOs;
using ExpenseControlAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControlAPI.Services;

/// <summary>
/// Serviço que encapsula a lógica de negócio relacionada a pessoas.
/// Gerencia criação, leitura, atualização e deleção de pessoas.
/// </summary>
public interface IPersonService
{
    Task<PersonResponse> CreatePersonAsync(CreatePersonRequest request);
    Task<List<PersonResponse>> GetAllPeopleAsync();
    Task<PersonResponse?> GetPersonByIdAsync(Guid id);
    Task<bool> DeletePersonAsync(Guid id);
}

public class PersonService : IPersonService
{
    private readonly ExpenseControlContext _context;

    public PersonService(ExpenseControlContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Cria uma nova pessoa no sistema.
    /// Valida que a idade seja um número positivo.
    /// </summary>
    /// <param name="request">Dados da pessoa a ser criada</param>
    /// <returns>Resposta com os dados da pessoa criada</returns>
    /// <exception cref="ArgumentException">Lançado se a idade for inválida</exception>
    public async Task<PersonResponse> CreatePersonAsync(CreatePersonRequest request)
    {
        // Validação: idade deve ser positiva
        if (request.Age <= 0)
            throw new ArgumentException("Age must be a positive number.");

        // Validação: nome não pode estar vazio
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name cannot be empty.");

        // Cria nova pessoa com ID gerado automaticamente
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Age = request.Age
        };

        // Adiciona à base de dados
        _context.People.Add(person);
        await _context.SaveChangesAsync();

        return MapToResponse(person);
    }

    /// <summary>
    /// Recupera todas as pessoas cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as pessoas</returns>
    public async Task<List<PersonResponse>> GetAllPeopleAsync()
    {
        var people = await _context.People
            .AsNoTracking()
            .ToListAsync();

        return people.Select(MapToResponse).ToList();
    }

    /// <summary>
    /// Recupera uma pessoa específica pelo seu ID.
    /// </summary>
    /// <param name="id">ID da pessoa a buscar</param>
    /// <returns>Dados da pessoa ou null se não encontrada</returns>
    public async Task<PersonResponse?> GetPersonByIdAsync(Guid id)
    {
        var person = await _context.People
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        return person != null ? MapToResponse(person) : null;
    }

    /// <summary>
    /// Deleta uma pessoa do sistema.
    /// Todos as transações associadas a essa pessoa serão deletadas automaticamente
    /// due à configuração de cascata no Entity Framework (DeleteBehavior.Cascade).
    /// </summary>
    /// <param name="id">ID da pessoa a deletar</param>
    /// <returns>true se deletado com sucesso, false se a pessoa não foi encontrada</returns>
    public async Task<bool> DeletePersonAsync(Guid id)
    {
        var person = await _context.People.FindAsync(id);
        if (person == null)
            return false;

        // Remove a pessoa (as transações serão removidas em cascata)
        _context.People.Remove(person);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Converte uma entidade Person em um DTO PersonResponse.
    /// </summary>
    private static PersonResponse MapToResponse(Person person)
    {
        return new PersonResponse
        {
            Id = person.Id,
            Name = person.Name,
            Age = person.Age
        };
    }
}
