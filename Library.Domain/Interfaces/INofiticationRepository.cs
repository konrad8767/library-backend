using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Library.Domain.Interfaces
{
    public interface INofiticationRepository
    {
        Task SendNotificationStatus(Book book, CancellationToken cancellationToken);
    }
}
