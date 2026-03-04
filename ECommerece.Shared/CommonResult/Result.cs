namespace ECommerece.Shared.CommonResult;

public class Result
{
    protected readonly List<Error> _errors = [];
    public bool IsSuccess => _errors.Count == 0; // if there is no error, then it is success
    public bool IsFailure => !IsSuccess; // if there is error, then it is failure
    public IReadOnlyList<Error> Errors => _errors;

    // ok - > success result
    protected Result()
    {

    }
    //fail with error
    protected Result(Error error)
    {
        _errors.Add(error);
    }
    //fail with errors
    protected Result(List<Error> errors)
    {
        _errors.AddRange(errors);
    }

    public static Result Ok()
    {
        return new Result();
    }
    public static Result Fail(Error error)
    {
        return new Result(error);
    }
    public static Result Fail(List<Error> errors)
    {
        return new Result(errors);
    }
}

public class Result<Tvalue> : Result
{
    private readonly Tvalue _value;
    public Tvalue Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access value of a failed result");
    // ok - > success result with value
    private Result(Tvalue value)
    {
        _value = value;
    }
    //fail with error
    private Result(Error error) : base(error)
    {
        _value = default!;
    }
    //fail with errors
    private Result(List<Error> errors) : base(errors)
    {
        _value = default!;
    }

    public static Result<Tvalue> Ok(Tvalue value)
    {
        return new Result<Tvalue>(value);
    }

    public static new Result<Tvalue> Fail(Error error)
    {
        return new Result<Tvalue>(error);
    }
    public static new Result<Tvalue> Fail(List<Error> errors)
    {
        return new Result<Tvalue>(errors);
    }

    public static implicit operator Result<Tvalue>(Tvalue value)
    {
        return Ok(value);
    }
    public static implicit operator Result<Tvalue>(Error error)
    {
        return Fail(error);
    }
    public static implicit operator Result<Tvalue>(List<Error> errors)
    {
        return Fail(errors);
    }
}
