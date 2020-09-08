using System;
using System.Threading.Tasks;

namespace GoFish.Web.Services
{
    public class GenericEventEmitter<TEventArgs> : IAsyncEventEmitter<TEventArgs> where TEventArgs : EventArgs
    {
        public GenericEventEmitter()
        {
            OnEvent = new AsyncEventCollection<TEventArgs>();
        }

        public AsyncEventCollection<TEventArgs> OnEvent { get; }

        public Task Trigger(object source, TEventArgs args)
        {
            return OnEvent.TriggerAsync(source, args);
        }
    }
}
