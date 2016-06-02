using System;
using System.ComponentModel;
using MvvmCross.Core.ViewModels;
using ReactiveUI;


namespace MvvmCross.ReactiveUI.Interop
{
    public abstract class MvxReactiveViewModel : MvxViewModel, IReactiveNotifyPropertyChanged<IReactiveObject>, IReactiveObject
    {
        readonly MvxReactiveObject reactiveObj = new MvxReactiveObject();
        bool suppressNpc;


        protected override MvxInpcInterceptionResult InterceptRaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            if (this.suppressNpc)
                return MvxInpcInterceptionResult.DoNotRaisePropertyChanged;

            return base.InterceptRaisePropertyChanged(changedArgs);
        }


        public virtual IDisposable SuppressChangeNotifications()
        {
            this.suppressNpc = true;
            var suppressor = this.reactiveObj.SuppressChangeNotifications();

            return new DisposableAction(() =>
            {
                this.suppressNpc = false;
                suppressor.Dispose();
            });
        }


        public virtual void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            this.reactiveObj.RaisePropertyChanging(args.PropertyName);
        }


        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing => this.reactiveObj.Changing;
        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed => this.reactiveObj.Changed;


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
