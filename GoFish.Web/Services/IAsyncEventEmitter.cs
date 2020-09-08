using System;
using System.Threading.Tasks;

namespace GoFish.Web.Services
{
    public interface IAsyncEventEmitter<TEventArgs> where TEventArgs : EventArgs
    {
        AsyncEventCollection<TEventArgs> OnEvent { get; }

        Task Trigger(object source, TEventArgs args);
    }
}