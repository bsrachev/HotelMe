﻿namespace HotelMe.Shared.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }

}
