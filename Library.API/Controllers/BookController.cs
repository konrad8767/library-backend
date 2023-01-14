using AutoMapper;
using Library.API.CQRS.Commands.Books;
using Library.API.CQRS.Models;
using Library.API.CQRS.Queries.Books;
using Library.API.DTO;
using Library.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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

        [HttpPost]
        [ProducesResponseType(typeof(CreateBookCommand.Response), 200)]
        public async Task<IActionResult> CreateBook([FromBody] BookDTO book, CancellationToken cancellationToken)
        {
            var request = new CreateBookCommand.Request
            {
                Book = book
            };

            var result = _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{bookId}")]
        [ProducesResponseType(typeof(DeleteBookCommand.Response), 200)]
        public async Task<IActionResult> DeleteBook([FromRoute] int bookId, CancellationToken cancellationToken)
        {
            var request = new DeleteBookCommand.Request
            {
                Id = bookId
            };

            var result = _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpPut]
        [Route("{bookId}")]
        [ProducesResponseType(typeof(UpdateBookCommand.Response), 200)]
        public async Task<IActionResult> UpdateBook([FromRoute] int bookId, [FromBody] BookDTO book, CancellationToken cancellationToken)
        {
            var request = new UpdateBookCommand.Request
            {
                Id = bookId,
                Book = book
            };

            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpPut]
        [Route("borrow/{bookId}")]
        [ProducesResponseType(typeof(BorrowBookCommand.Response), 200)]
        public async Task<IActionResult> BorrowBookById([FromRoute] int bookId, CancellationToken cancellationToken)
        {
            var request = new BorrowBookCommand.Request
            {
                BookId = bookId
            };

            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpPut]
        [Route("return/{bookId}")]
        [ProducesResponseType(typeof(ReturnBookCommand.Response), 200)]
        public async Task<IActionResult> ReturnBookById([FromRoute] int bookId, CancellationToken cancellationToken)
        {
            var request = new ReturnBookCommand.Request
            {
                BookId = bookId
            };

            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

    }
}
