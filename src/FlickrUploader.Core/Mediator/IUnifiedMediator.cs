using FlickrUploader.Core.Eventing;
using System.Threading.Tasks;

namespace FlickrUploader.Core.Mediator
{
    public interface IUnifiedMediator
    {
        /// <summary>
        /// Execute a command.
        /// </summary>
        /// <param name="command"></param>
        Task Execute(ICommand command);

        /// <summary>
        /// Executes a command returns ing a TResult
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<TResult> Execute<TResult>(ICommand<TResult> command);

        /// <summary>
        /// Publish a domain event.
        /// </summary>
        /// <param name="domainEvent"></param>
        void Publish(IDomainEvent domainEvent);

        /// <summary>
        /// Execute a query.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<TResult> Query<TResult>(IQuery<TResult> query);

    }
}