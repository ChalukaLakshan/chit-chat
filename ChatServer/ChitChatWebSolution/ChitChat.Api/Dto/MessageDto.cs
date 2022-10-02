using System;

namespace ChitChat.Api.Dto
{
    public class MessageDto
    {
        public string FromUsername { get; set; }
        public string ToUsername { get; set; }
        public string MessageText { get; set; }
        public DateTime Date { get; set; }
    }
}
