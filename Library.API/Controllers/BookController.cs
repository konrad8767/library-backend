using AutoMapper;
using Library.API.DTO;
using Library.Domain.Entities;
using Library.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Library.API.Controllers
{
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IMapper _mapper;
        public BookController(LibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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

        [HttpGet("{bookId}")]
        public ActionResult<BookDTO> Get([FromRoute] int bookId)
        {
            var book = _dbContext
                .Books
                .Include(x => x.Authors)
                .FirstOrDefault(x => x.Id == bookId);

            if (book is null)
            {
                return NotFound();
            }

            var bookDto = _mapper.Map<BookDTO>(book);

            return Ok(bookDto);
        }
    }
}
