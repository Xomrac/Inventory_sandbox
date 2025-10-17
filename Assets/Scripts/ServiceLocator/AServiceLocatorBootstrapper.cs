namespace XomracCore.Patterns.SL
{
	using UnityEngine;
	using Utils.Extensions;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(ServiceLocator))]
	public abstract class AServiceLocatorBootstrapper : MonoBehaviour
	{
		private ServiceLocator _target;
		internal ServiceLocator Target => _target.OrNull() ?? (_target = GetComponent<ServiceLocator>());

		private bool _hasBeenBootstrapped;

		void Awake() => BootstrapOnDemand();

		public void BootstrapOnDemand()
		{
			if (_hasBeenBootstrapped) return;
			_hasBeenBootstrapped = true;
			Bootstrap();
		}

		protected abstract void Bootstrap();
	}

}