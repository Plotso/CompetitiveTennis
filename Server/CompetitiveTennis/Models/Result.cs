﻿namespace CompetitiveTennis.Models;

using Microsoft.AspNetCore.Mvc;

public class Result
{
    private readonly List<string> _errors;
    
    public Result(bool isSuccess, List<string> errors)
    {
        IsSuccess = isSuccess;
        _errors = errors;
    }

    public bool IsSuccess { get; }

    public List<string> Errors 
        => IsSuccess ? new List<string>(0) : _errors;

    public static Result Success 
        => new(true, new List<string>(0));
    
    public static Result Failure(ErrorInfo error) => Failure(error.ToString());
    
    public static Result Failure(string error) => error;
    public static Result Failure(IEnumerable<ErrorInfo> errors) 
        => new(false, errors.Select(e => e.ToString()).ToList());
    public static Result Failure(IEnumerable<string> errors) 
        => new(false, errors.ToList());

    public static implicit operator Result(string error) 
        => Failure(new List<string> { error });

    public static implicit operator Result(List<string> errors) 
        => Failure(errors.ToList());

    public static implicit operator Result(bool success)
        => success ? Success : Failure(new[] { "Unsuccessful operation." });

    public static implicit operator bool(Result result)
        => result.IsSuccess;

    public static implicit operator ActionResult(Result result)
        => result.IsSuccess ? new OkResult() : new BadRequestObjectResult(result.Errors);
}

public class Result<TData> : Result
{
    private readonly TData _data;

    public Result(bool isSuccess, TData data, List<string> errors)
        : base(isSuccess, errors)
        => _data = data;

    public TData Data
        => IsSuccess
            ? _data
            : throw new InvalidOperationException(
                $"{nameof(Data)} is not available with a failed result. Use {Errors} instead.");

    public static Result<TData> SuccessWith(TData data) 
        => new(true, data, new List<string>());
    
    public new static Result<TData> Failure(IEnumerable<string> errors) 
        => new(false, default!, errors.ToList());

    public static implicit operator Result<TData>(string error)
        => Failure(new List<string> { error });

    public static implicit operator Result<TData>(List<string> errors)
        => Failure(errors);

    public static implicit operator Result<TData>(TData data)
        => SuccessWith(data);

    public static implicit operator bool(Result<TData> result)
        => result.IsSuccess;

    public static implicit operator ActionResult<TData>(Result<TData> result) 
        => result.IsSuccess ? result.Data : new BadRequestObjectResult(result.Errors);
}