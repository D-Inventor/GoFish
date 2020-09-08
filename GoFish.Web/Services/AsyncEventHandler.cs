using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace GoFish.Web.Services
{
    public class AsyncEventCollection<TEventModel>
    {
        private readonly ConcurrentDictionary<AsyncEventHandler<TEventModel>, object> _subscribers;

        public AsyncEventCollection()
        {
            _subscribers = new ConcurrentDictionary<AsyncEventHandler<TEventModel>, object>();
        }

        public void Subscribe(AsyncEventHandler<TEventModel> handler)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _subscribers.TryAdd(handler, default);
        }

        public void Unsubscribe(AsyncEventHandler<TEventModel> handler)
        {
            _subscribers.TryRemove(handler, out _);
        }

        public Task TriggerAsync(object sender, TEventModel args)
        {
            System.Collections.Generic.ICollection<AsyncEventHandler<TEventModel>> keys = _subscribers.Keys;
            return Task.WhenAll(keys.Select(k => k?.Invoke(sender, args)));
        }

        public static AsyncEventCollection<TEventModel> operator +(AsyncEventCollection<TEventModel> eventCollection, AsyncEventHandler<TEventModel> handler)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            eventCollection ??= new AsyncEventCollection<TEventModel>();
            eventCollection.Subscribe(handler);
            return eventCollection;
        }

        public static AsyncEventCollection<TEventModel> operator -(AsyncEventCollection<TEventModel> eventCollection, AsyncEventHandler<TEventModel> handler)
        {
            if (eventCollection is null)
            {
                throw new ArgumentNullException(nameof(eventCollection));
            }

            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            eventCollection.Subscribe(handler);
            return eventCollection;
        }
    }

    public delegate Task AsyncEventHandler<in TEventModel>(object sender, TEventModel args);
}
