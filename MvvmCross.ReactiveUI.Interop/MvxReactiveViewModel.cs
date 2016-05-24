using System;
using MvvmCross.Core.ViewModels;
using ReactiveUI;


namespace MvvmCross.ReactiveUI.Interop
{
    public abstract class MvxReactiveViewModel : MvxViewModel, IReactiveNotifyPropertyChanged<IReactiveObject>, IReactiveObject
    {
        readonly MvxReactiveObject reactiveObj = new MvxReactiveObject();


        public IDisposable SuppressChangeNotifications()
        {
            return this.reactiveObj.SuppressChangeNotifications();
        }

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing => this.reactiveObj.Changing;
        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed => this.reactiveObj.Changed;
        public virtual void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            this.reactiveObj.RaisePropertyChanging(args.PropertyName);
        }


        event PropertyChangingEventHandler IReactiveObject.PropertyChanging
        {
            add { this.reactiveObj.PropertyChanging += value; }
            remove { this.reactiveObj.PropertyChanging -= value; }
        }


        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add { this.reactiveObj.PropertyChanging += value; }
            remove { this.reactiveObj.PropertyChanging -= value; }
        }
    }
}
