using AutoMapper;
using FluentValidation;
using Library.API.DTO;
using Library.Domain.Entities;
using Library.Domain.Extensions;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Library.API.CQRS.Commands.Books
{
    public class UpdateBookCommand
    {
        public class Request : IRequest<Response>
        {
            public int Id { get; set; }
            public BookDTO Book { get; set; }
        }

        public class Response
        {
            public BookDTO Book { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            private readonly IBookRepository _bookRepository;

            public Validator(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;

                RuleFor(x => x.Book.Title.RemoveSpecialCharacters()).MinimumLength(1)
                    .WithMessage(ValidationErrorKeys.BookTitleCannotContainOnlySpecialChars);

                RuleFor(x => x.Book.Authors).NotEmpty()
                    .WithMessage(ValidationErrorKeys.BookMustHaveAtLeastOneAuthor);

                RuleFor(x => x.Book.PublicationDate).LessThanOrEqualTo(DateTime.UtcNow)
                    .WithMessage(ValidationErrorKeys.PublicationDateCannotBeLaterThanCurrent);

                RuleFor(x => x.Book.Version).GreaterThan(0)
                    .WithMessage(ValidationErrorKeys.VersionMustBeGreaterThanZero);
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IBookRepository _bookRepository;
            private readonly IMapper _mapper;

            public Handler(IBookRepository bookRepository, IMapper mapper)
            {
                _bookRepository = bookRepository;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var book = _mapper.Map<Book>(request.Book);
                var dbBook = await _bookRepository.GetBookById(request.Id, cancellationToken);

                dbBook.Update(book);

                await _bookRepository.UpdateBook(dbBook, cancellationToken);

                return new Response
                {
                    Book = _mapper.Map<BookDTO>(dbBook)
                };
            }
        }
    }
}
