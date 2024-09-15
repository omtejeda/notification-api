using NotificationService.Domain.Enums;
namespace NotificationService.Application.Exceptions;

public class RuleValidationException : Exception
{
    public RuleValidationException() {}
    public RuleValidationException(string message) : base(message) {}
    public RuleValidationException(ResultCode resultCode) : base (message: string.Empty) { Code = (int) resultCode; }
    public RuleValidationException(int code) : base(message: string.Empty) { Code = code; }
    public RuleValidationException(int code, string message) : base(message) { Code = code;  }
    public RuleValidationException(int code, string message, Exception inner) : base(message, inner) { Code = code; }

    public int Code { get; set; }
    public string Description => ((ResultCode) Code).ToString();
    private string InternalMessage => Description;
    public override string Message => string.IsNullOrWhiteSpace(base.Message) ? InternalMessage : base.Message;
    public override string StackTrace => $"{nameof(RuleValidationException)}: {InternalMessage} {this.Message}";
}