using MediatR;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.Base;

public interface ICommand : ICommand<Result>
{
}

public interface ICommand<out TResult> : IRequest<TResult> where TResult : Result
{
}

public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Result> where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult> where TResult : Result
{
}