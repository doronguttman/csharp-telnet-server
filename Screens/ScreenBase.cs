using System;
using System.Text;
using TelnetServer.Comm;

namespace TelnetServer.Screens
{
    abstract class ScreenBase : IData
    {
        public abstract string GetScreen();
        public abstract void ProcessPresentation(string presentation);

        #region Implementation of IData
        byte[] IData.GetBytes() => Encoding.ASCII.GetBytes(this.GetScreen());
        #endregion Implementation of IData
    }

    abstract class ScreenBase<T, TModel> : ScreenBase, IObserver<TModel> where T : IObservable<TModel>
    {
        protected ScreenBase(T model)
        {
            this.Model = model;
            model.Subscribe(this);
        }

        public T Model { get; set; }

        #region Implementation of IObserver<in TModel>
        /// <summary>Provides the observer with new data.</summary>
        /// <param name="value">The current notification information.</param>
        void IObserver<TModel>.OnNext(TModel value)
        {
            throw new NotImplementedException();
        }

        /// <summary>Notifies the observer that the provider has experienced an error condition.</summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        void IObserver<TModel>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        /// <summary>Notifies the observer that the provider has finished sending push-based notifications.</summary>
        void IObserver<TModel>.OnCompleted()
        {
            throw new NotImplementedException();
        }
        #endregion Implementation of IObserver<in TModel>
    }
}
