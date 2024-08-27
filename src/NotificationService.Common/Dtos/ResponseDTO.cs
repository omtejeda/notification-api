using System;
namespace NotificationService.Common.Dtos
{
    public class ResponseDTO
    {
        public bool? Success { get; set; }
        public int? Code { get; set; }
        public string Message { get; set; }
        public DateTime? Date { get; set; }
    }
}