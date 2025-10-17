namespace XomracCore.Bootstrap
{

	using Cysharp.Threading.Tasks;
	using UnityEngine;

	public interface IBootstrappable
	{
		UniTask Bootstrap();

		public void PrintSuccessMessage()
		{
			Debug.Log($"<color=green>{GetType().Name} booted successfully.</color>");
		}

		public void PrintStartingMessage()
		{
			Debug.Log($"<color=orange>Starting {GetType().Name} boot sequence.</color>");
		}

		public void PrintWarningMessage()
		{
			Debug.Log($"<color=red> in {GetType().Name} boot something went wrong!</color>");
		}
	}

}