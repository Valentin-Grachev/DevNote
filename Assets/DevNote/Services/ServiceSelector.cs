using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public class ServiceSelector : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> _environmentServices;
        [SerializeField] private List<MonoBehaviour> _saveServices;
        [SerializeField] private List<MonoBehaviour> _adsServices;
        [SerializeField] private List<MonoBehaviour> _purchaseServices;
        [SerializeField] private List<MonoBehaviour> _analyticsServices;
        [SerializeField] private List<MonoBehaviour> _reviewServices;
        


        public T GetServiceInterface<T>() where T : class
        {
            var service = typeof(T) switch
            {
                Type t when t == typeof(ISave) => GetAvailableService<ISave>(_saveServices) as T,
                Type t when t == typeof(IAds) => GetAvailableService<IAds>(_adsServices) as T,
                Type t when t == typeof(IReview) => GetAvailableService<IReview>(_reviewServices) as T,
                Type t when t == typeof(IEnvironment) => GetAvailableService<IEnvironment>(_environmentServices) as T,
                Type t when t == typeof(IAnalytics) => GetAvailableService<IAnalytics>(_analyticsServices) as T,
                Type t when t == typeof(IPurchase) => GetAvailableService<IPurchase>(_purchaseServices) as T,
                _ => null
            };

            if (service == null)
                throw new Exception($"{Const.LOG_PREFIX} Wrong service type : {nameof(T)}");


            return service;
        }

        private T GetAvailableService<T>(List<MonoBehaviour> services) where T : class
        {
            foreach (var service in services)
                if ((service as ISelectableService).Available) return service as T;

            throw new Exception($"{Const.LOG_PREFIX} Available service does'nt exist: {typeof(T)}");
        }


    }
}


