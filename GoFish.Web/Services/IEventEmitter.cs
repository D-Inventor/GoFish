using System;

namespace GoFish.Web.Services
{
    public interface IEventEmitter<TEventArgs> where TEventArgs : EventArgs
    {
        event EventHandler<TEventArgs> OnEvent;

        void Trigger(object source, TEventArgs args);
    }
}