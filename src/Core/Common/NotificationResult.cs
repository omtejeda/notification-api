namespace NotificationService.Core.Common
{    
    public class NotificationResult
    {
        public int Code { get; private set; }
        public string From { get; private set; }
        public bool SavesAttachments { get; private set; }
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public bool IsFailure => !IsSuccess;
        public int TriesCount { get; set; } = 0;

        public static NotificationResult Ok(int code)
            => new NotificationResult { Code = code, IsSuccess = true };

        public static NotificationResult Ok(int code, string message)
            => new NotificationResult { Code = code, IsSuccess = true, Message = message };

        public static NotificationResult Ok(int code, string message, string from, bool savesAttachments)
            => new NotificationResult { Code = code, IsSuccess = true, Message = message, From = from, SavesAttachments = savesAttachments };

        public static NotificationResult Fail(int code)
            => new NotificationResult { Code = code, IsSuccess = false };

        public static NotificationResult Fail(int code, string message)
            => new NotificationResult { Code = code, IsSuccess = false, Message = message };
    }
}