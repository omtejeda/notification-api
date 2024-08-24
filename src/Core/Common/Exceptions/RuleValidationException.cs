using System;
using NotificationService.Core.Common.Enums;
namespace NotificationService.Core.Common.Exceptions
{
    public class RuleValidationException : Exception
    {
        public RuleValidationException() {}
        public RuleValidationException(string message) : base(message) {}
        public RuleValidationException(ErrorCode errorCode) : base (message: string.Empty) { Code = (int) errorCode; }
        public RuleValidationException(int code) : base(message: string.Empty) { Code = code; }
        public RuleValidationException(int code, string message) : base(message) { Code = code;  }
        public RuleValidationException(int code, string message, Exception inner) : base(message, inner) { Code = code; }

        public int Code { get; set; }
        public string Description => ((ErrorCode) Code).ToString();
        private string InternalMessage => Description;
        public override string Message => string.IsNullOrWhiteSpace(base.Message) ? InternalMessage : base.Message;
        public override string StackTrace => $"{nameof(RuleValidationException)}: {InternalMessage} {this.Message}";
    }
}