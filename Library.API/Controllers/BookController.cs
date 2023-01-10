using AutoMapper;
using Library.API.CQRS.Models;
using Library.API.CQRS.Queries.Books;
using Library.API.DTO;
using Library.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public ActionResult<List<BookDTO>> GetAll()
        {
            var books = _dbContext
                .Books
                .Include(x => x.Authors)
                .ToList();

            var bookDtos = _mapper.Map<List<BookDTO>>(books);

            return Ok(bookDtos);
        }

        [HttpGet]
        [Route("{bookId}")]
        [ProducesResponseType(typeof(GetBookQuery.Response), 200)]
        public async Task<IActionResult> GetAllBooks([FromRoute] int bookId, CancellationToken cancellationToken)
        {
            var request = new GetBookQuery.Request
            {
                BookId = bookId
            };

            var result = await _mediator.Send(request, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        [Route("Books")]
        [ProducesResponseType(typeof(GetBooksQuery.Response), 200)]
        public async Task<IActionResult> GetBooks([FromBody] SearchBookModel model, CancellationToken cancellationToken)
        {
            var request = new GetBooksQuery.Request
            {
                Filters = model.Filters,
                SortingField = model.SortingField,
                IsDesc = model.IsDesc
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }
    }
}
