using System;

namespace GoFish.Web.Services
{
    public class GenericEventEmitter<TEventArgs> : IEventEmitter<TEventArgs> where TEventArgs : EventArgs
    {
        public event EventHandler<TEventArgs> OnEvent;
        public void Trigger(object source, TEventArgs args)
        {
            var handler = OnEvent;
            handler?.Invoke(source, args);
        }
    }
}
