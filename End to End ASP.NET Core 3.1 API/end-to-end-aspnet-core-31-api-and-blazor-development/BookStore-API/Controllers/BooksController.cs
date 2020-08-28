using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore_API.Contracts;
using BookStore_API.Data;
using BookStore_API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore_API.Controllers
{
    /// <summary>
    /// Books
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class BooksController : ControllerBase
    {
        readonly IBookRepository _repository;
        readonly IMapper _mapper;
        readonly ILoggerService _logger;
        public BooksController(IBookRepository repository, IMapper mapper, ILoggerService logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get Books
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var location = GetControllerAndActionNames();
            try
            {
                var books = await _repository.FindAll();
                var response = _mapper.Map<IList<BookDTO>>(books);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{location} : {ex.Message}-{ex.InnerException}");
            }
        }

        private IActionResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong! please contact Admin");
        }

        /// <summary>
        /// Get Book
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBook(int id)
        {
            var location = GetControllerAndActionNames();
            try
            {
                var book = await _repository.FindById(id);
                if (book == null)
                {
                    return NotFound();
                }
                var response = _mapper.Map<BookDTO>(book);
                return Ok(response);
            }
            catch (Exception ex)
            {

                return InternalError($"{location} : {ex.Message}-{ex.InnerException}");
            }
        }

        /// <summary>
        /// Create a book
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO bookDTO)
        {
            var location = GetControllerAndActionNames();
            try
            {
                if (bookDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(bookDTO);
                var isSuccess = await _repository.Create(book);
                if (!isSuccess)
                {
                    return InternalError($"{location} - Book Creation is failed");
                }
                return Created("Create", book);
            }
            catch (Exception ex)
            {

                return InternalError($"{location} : {ex.Message}-{ex.InnerException}");
            }
        }
        /// <summary>
        /// Update a Book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO bookDTO)
        {
            var location = GetControllerAndActionNames();
            try
            {
                if (id < 1 || bookDTO == null || id != bookDTO.Id)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(bookDTO);
                var isSuccess = await _repository.Update(book);
                if (!isSuccess)
                {
                    return InternalError($"{location} - Book Updation is failed");
                }
                return NoContent();
            }
            catch (Exception ex)
            {

                return InternalError($"{location} : {ex.Message}-{ex.InnerException}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerAndActionNames();
            try
            {
                if (id < 1)
                {
                    return BadRequest();
                }
                var book = await _repository.FindById(id);
                if (book == null)
                {
                    return NotFound();
                }
                var isSuccess = await _repository.Delete(book);
                if (!isSuccess)
                {
                    return InternalError($"{location} - Book Deletion is failed");
                }
                return NoContent();
            }
            catch (Exception ex)
            {

                return InternalError($"{location} : {ex.Message}-{ex.InnerException}");
            }
        }
        private string GetControllerAndActionNames()
        {
            var controllerName = ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            return $"{controllerName} - {actionName}";
        }

    }
}
