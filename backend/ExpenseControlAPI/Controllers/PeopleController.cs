using ExpenseControlAPI.DTOs;
using ExpenseControlAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControlAPI.Controllers;

/// <summary>
/// Controller que gerencia os endpoints relacionados a pessoas.
/// Fornece endpoints para CRUD de pessoas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly IPersonService _personService;

    public PeopleController(IPersonService personService)
    {
        _personService = personService;
    }

    /// <summary>
    /// GET: api/people
    /// Lista todas as pessoas cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de pessoas com status 200 OK</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PersonResponse>>>> GetAll()
    {
        try
        {
            var people = await _personService.GetAllPeopleAsync();
            return Ok(new ApiResponse<List<PersonResponse>>
            {
                Success = true,
                Message = "People retrieved successfully",
                Data = people
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<PersonResponse>>
            {
                Success = false,
                Message = $"Error retrieving people: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }

    /// <summary>
    /// GET: api/people/{id}
    /// Recupera uma pessoa específica pelo seu ID.
    /// </summary>
    /// <param name="id">ID da pessoa a recuperar</param>
    /// <returns>Dados da pessoa ou 404 se não encontrada</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<PersonResponse>>> GetById(Guid id)
    {
        try
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
            {
                return NotFound(new ApiResponse<PersonResponse>
                {
                    Success = false,
                    Message = "Person not found",
                    ErrorCode = "PERSON_NOT_FOUND"
                });
            }

            return Ok(new ApiResponse<PersonResponse>
            {
                Success = true,
                Message = "Person retrieved successfully",
                Data = person
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<PersonResponse>
            {
                Success = false,
                Message = $"Error retrieving person: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }

    /// <summary>
    /// POST: api/people
    /// Cria uma nova pessoa no sistema.
    /// </summary>
    /// <param name="request">Dados da pessoa a criar (nome e idade)</param>
    /// <returns>Dados da pessoa criada com status 201 Created</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PersonResponse>>> Create([FromBody] CreatePersonRequest request)
    {
        try
        {
            var person = await _personService.CreatePersonAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = person.Id }, new ApiResponse<PersonResponse>
            {
                Success = true,
                Message = "Person created successfully",
                Data = person
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<PersonResponse>
            {
                Success = false,
                Message = ex.Message,
                ErrorCode = "VALIDATION_ERROR"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<PersonResponse>
            {
                Success = false,
                Message = $"Error creating person: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }

    /// <summary>
    /// DELETE: api/people/{id}
    /// Deleta uma pessoa do sistema.
    /// Automaticamente deleta todas as transações associadas a essa pessoa.
    /// </summary>
    /// <param name="id">ID da pessoa a deletar</param>
    /// <returns>Status 200 OK se deletado com sucesso, 404 se não encontrado</returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
    {
        try
        {
            var deleted = await _personService.DeletePersonAsync(id);
            if (!deleted)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Person not found",
                    ErrorCode = "PERSON_NOT_FOUND"
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Person deleted successfully. Associated transactions were also deleted.",
                Data = id.ToString()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<string>
            {
                Success = false,
                Message = $"Error deleting person: {ex.Message}",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }
}
