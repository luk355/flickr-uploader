using FlickrUploader.Core.Eventing;

namespace FlickrUploader.Core.Mediator
{
    public interface IUnifiedMediator
    {
        /// <summary>
        /// Execute a command.
        /// </summary>
        /// <param name="command"></param>
        void Execute(ICommand command);

        /// <summary>
        /// Executes a command returns ing a TResult
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        TResult Execute<TResult>(ICommand<TResult> command);

        /// <summary>
        /// Publish a domain event.
        /// </summary>
        /// <param name="domainEvent"></param>
        void Publish(IDomainEvent domainEvent);

        ///// <summary>
        ///// Publish a system event.
        ///// </summary>
        ///// <param name="systemEvent"></param>
        //void Publish(ISystemEvent systemEvent);

        ///// <summary>
        ///// Publish a system event asynchronously.
        ///// </summary>
        ///// <param name="systemEvent"></param>
        ///// <returns></returns>
        //Task PublishAsync(IAsyncSystemEvent systemEvent);

        /// <summary>
        /// Execute a query.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        TResult Query<TResult>(IQuery<TResult> query);

    }
}