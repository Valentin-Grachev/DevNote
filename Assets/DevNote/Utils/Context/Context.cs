using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public class Context : MonoBehaviour
    {
        private static List<IHolder> _holders = new();

        private static Dictionary<Type, object> _registers = new();

        private static List<IStartable> _startables = new();
        private static List<IUpdatable> _updatables = new();
        private static List<IFixedUpdatable> _fixedUpdatables = new();
        private static List<IContextDisposable> _disposables = new();


        public static void Register<T>(T instance) where T : class
        {
            var instanceType = typeof(T);

            if (_registers.ContainsKey(instanceType))
                throw new Exception($"{Const.LOG_PREFIX} Context: type {instanceType.Name} is already registered");

            _registers.Add(instanceType, instance);

            if (instance is IStartable) _startables.Add(instance as IStartable);
            if (instance is IUpdatable) _updatables.Add(instance as IUpdatable);
            if (instance is IFixedUpdatable) _fixedUpdatables.Add(instance as IFixedUpdatable);
            if (instance is IContextDisposable) _disposables.Add(instance as IContextDisposable);

            foreach (var holder in _holders)
            {
                if (holder.RequireType(instanceType))
                    (holder as Holder<T>).Resolve(instance);
            }
        }

        public static T Get<T>() where T : class
        {
            if (_registers.TryGetValue(typeof(T), out var instance))
                return (T)instance;

            return null;
        }

        public static void RegisterHolder<T>(Holder<T> holder) where T : class
        {
            var instance = Get<T>();

            if (instance == null)
                _holders.Add(holder);

            else holder.Resolve(instance);
        }

        public static void Unregister(Type type)
        {
            var instance = _registers[type];

            if (instance is IStartable) _startables.Remove(instance as IStartable);
            if (instance is IUpdatable) _updatables.Remove(instance as IUpdatable);
            if (instance is IFixedUpdatable) _fixedUpdatables.Remove(instance as IFixedUpdatable);
            if (instance is IContextDisposable)
            {
                var disposable = instance as IContextDisposable;
                disposable.Dispose();
                _disposables.Remove(disposable);
            }

            _registers.Remove(type);
        }


        private void Start()
        {
            for (int i = 0; i < _startables.Count; i++)
                _startables[i].Start();
        }

        private void Update()
        {
            for (int i = 0; i < _updatables.Count; i++)
                _updatables[i].Update();
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _fixedUpdatables.Count; i++)
                _fixedUpdatables[i].FixedUpdate();
        }


    }

}

