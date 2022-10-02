using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Data.EfCore
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid FromUserId { get; set; }
        public User FromUser { get; set; }
        public string MessageText { get; set; }
        public DateTime Date { get; set; }

        [NotMapped] public string FromUserName { get; set; }
    }
}
