using AutoMapper;
using Library.API.CQRS.Commands.Books;
using Library.API.CQRS.Models;
using Library.API.CQRS.Queries.Books;
using Library.API.DTO;
using Library.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Library.API.Controllers
{
    [ApiController]
    [Authorize]
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

            if (result.Book == null)
            {
                return BadRequest(result);
            }

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

        [HttpGet]
        [Route("MyBooks/{userId}")]
        [ProducesResponseType(typeof(GetMyBooksQuery.Response), 200)]
        public async Task<IActionResult> GetMyBooks([FromRoute] int userId, CancellationToken cancellationToken)
        {
            var request = new GetMyBooksQuery.Request
            {
                UserId = userId
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

            var result = await _mediator.Send(request, cancellationToken);
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

            var result = await _mediator.Send(request, cancellationToken);

            if (result.Success == false)
            {
                return BadRequest(result);
            }

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

            if (result.Book is null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("borrow")]
        [ProducesResponseType(typeof(BorrowBookCommand.Response), 200)]
        public async Task<IActionResult> BorrowBookById([FromBody] BorrowBookCommand.Request incomingRequest, CancellationToken cancellationToken)
        {
            var request = new BorrowBookCommand.Request
            {
                BookId = incomingRequest.BookId,
                UserId = incomingRequest.UserId
            };

            var result = await _mediator.Send(request, cancellationToken);

            if (result.Success == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("return")]
        [ProducesResponseType(typeof(ReturnBookCommand.Response), 200)]
        public async Task<IActionResult> ReturnBookById([FromBody] ReturnBookCommand.Request incomingRequest, CancellationToken cancellationToken)
        {
            var request = new ReturnBookCommand.Request
            {
                BookId = incomingRequest.BookId
            };

            var result = await _mediator.Send(request, cancellationToken);

            if (result.Success == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("spectate")]
        [ProducesResponseType(typeof(SpectateBookCommand.Response), 200)]
        public async Task<IActionResult> SpectateBookById([FromBody] SpectateBookCommand.Request incomingRequest, CancellationToken cancellationToken)
        {
            var request = new SpectateBookCommand.Request
            {
                BookId = incomingRequest.BookId,
                UserId = incomingRequest.UserId
            };

            var result = await _mediator.Send(request, cancellationToken);

            if (result.Success == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
