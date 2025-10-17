namespace XomracCore.Patterns.SL
{
	using UnityEngine;
	public class ServiceLocatorGlobalBootstrap : AServiceLocatorBootstrapper
	{
		[SerializeField] private bool _dontDestroyOnLoad = true;
		protected override void Bootstrap()
		{
			Target.MakeGlobal(_dontDestroyOnLoad);
		}
	}

}