using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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


        public event PropertyChangingEventHandler PropertyChanging
        {
            add { this.reactiveObj.PropertyChanging += value; }
            remove { this.reactiveObj.PropertyChanging -= value; }
        }

        public new bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            var original = storage;
            IReactiveObjectExtensions.RaiseAndSetIfChanged(this, ref storage, value, propertyName);

            return !EqualityComparer<T>.Default.Equals(original, value);
        }
    }
    
    public abstract class MvxReactiveViewModel<T, TReturn> : MvxViewModel<T, TReturn>, IReactiveNotifyPropertyChanged<IReactiveObject>,
        IReactiveObject, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private readonly MvxReactiveObject reactiveObj = new MvxReactiveObject();
        private bool suppressNpc;

        protected override MvxInpcInterceptionResult InterceptRaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            if (this.suppressNpc)
                return MvxInpcInterceptionResult.DoNotRaisePropertyChanged;
            return base.InterceptRaisePropertyChanged(changedArgs);
        }

        public virtual IDisposable SuppressChangeNotifications()
        {
            this.suppressNpc = true;
            IDisposable suppressor = this.reactiveObj.SuppressChangeNotifications();
            return (IDisposable) new DisposableAction((Action) (() =>
            {
                this.suppressNpc = false;
                suppressor.Dispose();
            }));
        }

        public virtual void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            this.reactiveObj.RaisePropertyChanging<MvxReactiveObject>(args.PropertyName);
        }

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing => this.reactiveObj.Changing;

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed => this.reactiveObj.Changed;

        public event PropertyChangingEventHandler PropertyChanging
        {
            add { this.reactiveObj.PropertyChanging += value; }
            remove { this.reactiveObj.PropertyChanging -= value; }
        }

        public new bool SetProperty<TStore>(ref TStore storage, TStore value, [CallerMemberName] string propertyName = null)
        {
            TStore x = storage;
            IReactiveObjectExtensions.RaiseAndSetIfChanged(this, ref storage, value,
                propertyName);
            return !EqualityComparer<TStore>.Default.Equals(x, value);
        }
    }
}
