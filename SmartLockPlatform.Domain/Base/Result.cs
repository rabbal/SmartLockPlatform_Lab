using System.Diagnostics.CodeAnalysis;

namespace SmartLockPlatform.Domain.Base;

public class Result
{
    private static readonly Result _ok = new(false, string.Empty, string.Empty);
    private readonly List<ValidationFailure> _failures;

    protected Result(bool failed, string message, string? details) : this(failed, message, details,
        Enumerable.Empty<ValidationFailure>())
    {
    }

    protected Result(bool failed, string message, string? details, IEnumerable<ValidationFailure> failures)
    {
        Failed = failed;
        Message = message;
        Details = details;

        _failures = failures.ToList();
    }

    public virtual bool Failed { get; }
    public bool Forbidden { get; private set; }
    public string Message { get; }
    public string? Details { get; }
    public IEnumerable<ValidationFailure> Failures => _failures.AsReadOnly();

    public Result WithFailure(string memberName, string message)
    {
        if (!Failed) throw new InvalidOperationException("Can not add failure to ok result!");

        _failures.Add(new ValidationFailure(memberName, message));
        return this;
    }

    public static Result Ok() => _ok;

    public static Result Ok(string message)
    {
        return new Result(false, message, null);
    }

    public static Result Ok(string message, string? details)
    {
        return new Result(false, message, details);
    }

    public static Result Fail(string message, string? details = null)
    {
        return new Result(true, message, details);
    }
    
    public static Result Forbid(string message, string? details = null)
    {
        var result = Fail(message, details);
        result.Forbidden = true;
        return result;
    }
    public static Result Fail(string message, IEnumerable<ValidationFailure> failures)
    {
        return new Result(true, message, string.Empty, failures);
    }

    public static Result<T> Fail<T>(string message, string? details = null)
    {
        return new Result<T>(default, true, message, details);
    }

    public static Result<T> Fail<T>(string message, IEnumerable<ValidationFailure> failures)
    {
        return new Result<T>(default, true, message, string.Empty, failures);
    }

    public static Result<T> Fail<T>(string message, string details, IEnumerable<ValidationFailure> failures)
    {
        return new Result<T>(default, true, message, details, failures);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, false, string.Empty, string.Empty);
    }

    public static Result<T> Ok<T>(T value, string message, string? details = null)
    {
        return new Result<T>(value, false, message, details);
    }

    public static Result Combine(string symbol, params Result[] results)
    {
        var failedList = results.Where(x => !x.Failed).ToList();

        if (!failedList.Any()) return Ok();

        var message = string.Join(symbol, failedList.Select(x => x.Message).ToArray());
        var failures = failedList.SelectMany(r => r.Failures).ToList();

        return Fail(message, failures);
    }

    public static Result Combine(params Result[] results)
    {
        return Combine(", ", results);
    }

    public static Result Combine<T>(params Result<T>[] results)
    {
        return Combine(", ", results);
    }

    public static Result Combine<T>(string symbol, params Result<T>[] results)
    {
        var untyped = results.Select(result => (Result)result).ToArray();
        return Combine(symbol, untyped);
    }

    public override string ToString()
    {
        return !Failed
            ? "Ok"
            : $"Failed: {Message}";
    }
}

public class Result<T> : Result
{
    private readonly T? _data;

    protected internal Result(T? data, bool failed, string message, string? details)
        : base(failed, message, details)
    {
        if (!failed && data is null)
        {
            throw new ArgumentNullException(nameof(data), "The value must be provided for ok result");
        }
        
        _data = data;
    }

    protected internal Result(T? data, bool failed, string message, string? details,
        IEnumerable<ValidationFailure> failures)
        : base(failed, message, details, failures)
    {
        _data = data;
    }

    [MemberNotNullWhen(false, nameof(Data))]
    public override bool Failed => base.Failed;

    public T? Data => !Failed ? _data : throw new InvalidOperationException("There is no value for failure.");

    public static implicit operator Result<T>(T value)
    {
        return Ok(value);
    }
}