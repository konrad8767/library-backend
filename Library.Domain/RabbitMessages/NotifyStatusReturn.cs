using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.RabbitMessages
{
    public class NotifyStatusReturn
    {
        public string NotificationType { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientLogin { get; set; }
        public DateTime SentDate { get; set; }
        public string BookTitle { get; set; }
    }
}
