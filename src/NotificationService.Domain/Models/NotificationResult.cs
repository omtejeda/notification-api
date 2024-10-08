﻿namespace NotificationService.Domain.Models
{    
    public class NotificationResult
    {
        public int Code { get; private set; }
        public string From { get; private set; } = string.Empty;
        public bool SavesAttachments { get; private set; }
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public bool IsFailure => !IsSuccess;
        public int TriesCount { get; set; } = 0;

        public static NotificationResult Ok(int code)
            => new() { Code = code, IsSuccess = true };

        public static NotificationResult Ok(int code, string message)
            => new() { Code = code, IsSuccess = true, Message = message };

        public static NotificationResult Ok(int code, string message, string from, bool savesAttachments)
            => new() { Code = code, IsSuccess = true, Message = message, From = from, SavesAttachments = savesAttachments };

        public static NotificationResult Fail(int code)
            => new() { Code = code, IsSuccess = false };

        public static NotificationResult Fail(int code, string message)
            => new() { Code = code, IsSuccess = false, Message = message };
    }
}