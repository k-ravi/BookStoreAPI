using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore_API.Contracts;
using BookStore_API.Data;
using BookStore_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore_API.Controllers
{
    /// <summary>
    /// Endpoints used to interact with the authors in the book store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AuthorsController : ControllerBase
    {
        readonly IAuthorRepository authorRepository;
        readonly ILoggerService _logger;
        readonly IMapper _mapper;
        public AuthorsController(IAuthorRepository author, ILoggerService logger, IMapper mapper)
        {
            authorRepository = author;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all authors
        /// </summary>
        /// <returns>List Of Authors</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                _logger.LogInfo("Attempted Get all Authors");
                var authors = await authorRepository.FindAll();
                var response = _mapper.Map<IList<AuthorDTO>>(authors);
                _logger.LogInfo("Successfully Got all Authors");
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}- {ex.InnerException}");
            }
        }

        private IActionResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something wrong. Please contact the Admin");
        }

        /// <summary>
        /// Gets  author by id
        /// </summary>
        /// <returns>Author</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthor(int id)
        {
            try
            {
                _logger.LogInfo("Attempted Get  Author");
                var author = await authorRepository.FindById(id);
                if (author == null)
                {
                    _logger.LogWarn("Author not found");
                    return NotFound();
                }
                var response = _mapper.Map<AuthorDTO>(author);
                _logger.LogInfo("Successfully Got  Author");
                return Ok(author);
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}- {ex.InnerException}");
            }
        }

        /// <summary>
        /// Create an author
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorDTO)
        {
            try
            {
                _logger.LogInfo("Author Creation Method is called");
                if (authorDTO == null)
                {
                    _logger.LogWarn("Empty Request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Author author = _mapper.Map<Author>(authorDTO);
                var isSuccess = await authorRepository.Create(author);
                if (!isSuccess)
                {
                    return InternalError("Author Creation is failed");
                }
                return Created("Create", new { author });
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}- {ex.InnerException}");
            }
        }

        /// <summary>
        /// Update an author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDTO authorDTO)
        {
            try
            {
                if (id < 1 || authorDTO == null || id != authorDTO.Id)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var isExists = await authorRepository.IsExists(id);
                if (!isExists)
                {
                    return NotFound();
                }
                var author = _mapper.Map<Author>(authorDTO);
                var issuccess = await authorRepository.Update(author);
                if (!issuccess)
                {
                    return InternalError($"Update Author is failed");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}- {ex.InnerException}");
            }
        }

        /// <summary>
        /// Update an author
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var author = await authorRepository.FindById(id);
                if (author == null)
                {
                    return NotFound();
                }
                var issuccess = await authorRepository.Delete(author);
                if (!issuccess)
                {
                    return InternalError($"Delete Author is failed");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}- {ex.InnerException}");
            }
        }
    }
}
