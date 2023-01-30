using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Domain.RabbitMessages;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Infrastructure.RepositoryImplementation
{
    public class NotificationRepository : INofiticationRepository
    {
        readonly IBus _bus;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly LibraryDbContext _dbContext;
        private readonly string _rabbitHostName;

        public NotificationRepository(IConfiguration config, 
            IBus bus, 
            IBookRepository bookRepository, 
            IUserRepository userRepository,
            LibraryDbContext dbContext)
        {
            
            _bus = bus;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _dbContext = dbContext;
            var rabbitConfigSection = config.GetSection("Rabbit");
            _rabbitHostName = rabbitConfigSection["HostName"];
        }

        public async Task SendNotificationStatus(Book book, CancellationToken cancellationToken)
        {
            var endpoint = await _bus.GetSendEndpoint(new System.Uri($"rabbitmq://{_rabbitHostName}/library-notifications"));
            var bookSpectators = await _userRepository.GetSpectatorsByBookId(book.Id, cancellationToken);

            foreach (var user in bookSpectators)
            {
                NotifyStatusReturn rabbitMessage = new NotifyStatusReturn
                {
                    NotificationType = NotificationTypes.BookReturn,
                    RecipientAddress = user.EmailAddress,
                    RecipientLogin = user.Login,
                    SentDate = DateTime.UtcNow,
                    BookTitle = book.Title
                };
                await endpoint.Send(rabbitMessage);
            }
        }
    }


}
