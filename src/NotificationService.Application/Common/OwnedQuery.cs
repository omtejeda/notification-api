namespace NotificationService.Application.Common;

public abstract record OwnedQuery
{
    private string? _owner;
    
    public string? GetOwner() => _owner;
    public void SetOwner(string? owner) => _owner = owner;
}