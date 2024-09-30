using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BioEngineerLab.Configurations;
using UnityEngine;

namespace BioEngineerLab.Core
{
    public class Engine
    {
        public static bool IsInitialized { get; private set; } = false;
        public static RuntimeBehaviour Behaviour { get; private set; }

        private static Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            Behaviour = new GameObject("RuntimeBehaviour", typeof(RuntimeBehaviour)).GetComponent<RuntimeBehaviour>();
            
            SaveService saveService = new SaveService();
            TasksService tasksService = new TasksService(saveService);
            
            AddService(saveService);
            AddService(tasksService);
            AddService(new SubstancesService(GetConfiguration<SubstancesConfiguration>(), tasksService));
            AddService(new InputService(GetConfiguration<InputConfiguration>()));
            AddService(new HandModelsService(GetConfiguration<HandModelConfiguration>()));

            foreach(var pair in _services)
            {
                pair.Value.Initialize();
            }
        }
        public static void OnDestroy()
        {
            foreach (var service in _services.Values)
                service.Destroy();
            
            _services.Clear();
        }

        public static T Instantiate<T>(T prototype, Transform parent = default) where T : UnityEngine.Object
        {
            if (Behaviour is null)
                throw new Exception("Engine is not initialized");

            var newObj = UnityEngine.Object.Instantiate(prototype, parent ? parent : Behaviour.transform);
            return newObj;
        }

        public static GameObject CreateObject(string name = default, Transform parent = default,
            params Type[] components)
        {
            if (Behaviour is null)
                throw new Exception("Engine is not initialized");

            var objName = name ?? "PlatformObject";
            GameObject newObj = components != null ? new GameObject(objName, components) : new GameObject(objName);
            newObj.transform.SetParent(parent ? parent : Behaviour.transform);

            return newObj;
        }

        public static void Destroy<T>(T obj, float seconds = 0) where T: UnityEngine.Object
        {
            if (Behaviour is null)
                throw new Exception("Engine is not initialized");
            
            UnityEngine.Object.Destroy(obj, seconds);
        }
        
        public static void AddService<T>(T service) where T: IService
        {
            if (_services.ContainsKey(typeof(T)))
                throw new Exception($"Service {typeof(T)} already exists");
                
            _services.Add(typeof(T), service);
        }

        public static void RemoveService<T>() where T: IService
        {
            if (_services.ContainsKey(typeof(T)))
                _services.Remove(typeof(T));
            else
                throw new Exception($"Service {typeof(T)} doesn't exists");
        }

        /// <summary>
        /// Функция возвращает сервис
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetService<T>() where T : class, IService
        {
            if (_services.ContainsKey(typeof(T)))
                return (T) _services[typeof(T)];

            Type type = typeof(T);
            var result = _services.FirstOrDefault(x => type.IsInstanceOfType(x.Value));

            if (result.Value is null)
                throw new Exception($"Service {typeof(T)} doesn't exists");

            return (T) result.Value;
        }

        public static void StartCoroutine(IEnumerator enumerator)
        {
            if (Behaviour is null)
                throw new Exception("Engine is not initialized");

            Behaviour.StartCoroutine(enumerator);
        }

        public static void StopCoroutine(IEnumerator enumerator)
        {
            if (Behaviour is null)
                throw new Exception("Engine is not initialized");

            Behaviour.StopCoroutine(enumerator);
        }
        
        public static T GetConfiguration<T>() where T : Configuration
        {
            return ResourcesLoader.GetConfiguration<T>();
        }
    }
}
