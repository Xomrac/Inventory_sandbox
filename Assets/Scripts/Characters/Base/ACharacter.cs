namespace XomracCore.Characters.Base
{
	using System.Collections.Generic;
	using UnityEngine;
	using Patterns.SL;

	public abstract class ACharacter : MonoBehaviour
	{
		[SerializeField] private List<Object> _services;

		private ServiceLocator _serviceLocator;
		public ServiceLocator ServiceLocator => _serviceLocator;

		private void Awake()
		{
			_serviceLocator = ServiceLocator.Of(this);
			foreach (Object service in _services)
			{
				_serviceLocator.AddService(service.GetType(), service);
			}
			Debug.Log("Registered " + _services.Count + " services for " + gameObject.name);
		}
	}
}