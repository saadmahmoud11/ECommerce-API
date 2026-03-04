namespace ECommerece.Shared.CommonResult;

public class Error
{
    public string Code { get; set; }
    public string Description { get; set; }
    public ErrorType Type { get; set; }
    private Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }
    // static factory method to create error
    public static Error Failure(string code = "General.Failure", string description = "General.Failure has occurs")
    {
        return new Error(code, description, ErrorType.Failure);
    }
    public static Error Validation(string code = "General.Validation", string description = "Validation error has occurs")
    {
        return new Error(code, description, ErrorType.Validation);
    }
    public static Error NotFound(string code = "General.NotFound", string description = "Not found error has occurs")
    {
        return new Error(code, description, ErrorType.NotFound);
    }
    public static Error Unauthorized(string code = "General.Unauthorized", string description = "Unauthorized error has occurs")
    {
        return new Error(code, description, ErrorType.Unauthorized);
    }
    public static Error Forbidden(string code = "General.Forbidden", string description = "Forbidden error has occurs")
    {
        return new Error(code, description, ErrorType.Forbidden);
    }
    public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "Invalid credentials error has occurs")
    {
        return new Error(code, description, ErrorType.InvalidCredentials);
    }
}
