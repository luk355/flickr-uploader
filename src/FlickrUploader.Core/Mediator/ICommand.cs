using MediatR;

namespace FlickrUploader.Core.Mediator
{
    public interface ICommand : IRequest<Unit>
    {
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }


    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Unit> where TCommand : ICommand
    {
    }

    public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
    }
}
