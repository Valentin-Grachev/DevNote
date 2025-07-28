using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public class Context : MonoBehaviour
    {
        private static Context _instance;

        private Dictionary<Type, object> _registers = new();

        private List<IStartable> _startables = new();
        private List<IUpdatable> _updatables = new();
        private List<IFixedUpdatable> _fixedUpdatables = new();
        private List<IContextDisposable> _disposables = new();


        public void Initialize() => _instance = this;

        public static void Register<T>(T instance) where T : class
        {
            print($"Register {typeof(T)}");
            var type = typeof(T);

            if (_instance._registers.ContainsKey(type))
                throw new Exception($"{Const.LOG_PREFIX} Context: type {type.Name} is already registered");

            _instance._registers[type] = instance;

            if (instance is IStartable) _instance._startables.Add(instance as IStartable);
            if (instance is IUpdatable) _instance._updatables.Add(instance as IUpdatable);
            if (instance is IFixedUpdatable) _instance._fixedUpdatables.Add(instance as IFixedUpdatable);
            if (instance is IContextDisposable) _instance._disposables.Add(instance as IContextDisposable);
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);

            if (_instance._registers.TryGetValue(type, out var instance))
                return (T)instance;

            throw new Exception($"{Const.LOG_PREFIX} Context: type {type.Name} is not registered");
        }

        public static void Unregister(Type type)
        {
            var instance = _instance._registers[type];

            if (instance is IStartable) _instance._startables.Remove(instance as IStartable);
            if (instance is IUpdatable) _instance._updatables.Remove(instance as IUpdatable);
            if (instance is IFixedUpdatable) _instance._fixedUpdatables.Remove(instance as IFixedUpdatable);
            if (instance is IContextDisposable) _instance._disposables.Remove(instance as IContextDisposable);

            _instance._registers.Remove(type);
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

        private void OnDestroy()
        {
            for (int i = 0; i < _disposables.Count; i++)
                _disposables[i].Dispose();
        }


    }

}

