namespace XomracCore.Audio
{

	using UnityEngine.Audio;
	using UnityEngine;
	using UnityEngine.Pool;
	using Patterns.SL;
	using System.Collections.Generic;
	using Cysharp.Threading.Tasks;
	using Bootstrap;

	public class SoundSystem : MonoBehaviour, IBootstrappable
	{
		[SerializeField] private int _maxSfxSources = 10;
		[SerializeField] private bool _prewarmSfxPool = true;
		[SerializeField] private SFXPlayer _sfxPlayerPrefab;

		private ObjectPool<SFXPlayer> _sfxPool;
		public ObjectPool<SFXPlayer> SfxPool => _sfxPool;

		public async UniTask Bootstrap()
		{
			ServiceLocator.Global.AddService(this);
			SetupPool();
			PreWarmPool();
			await UniTask.Yield();
		}

		public void PlaySFX(AudioResource clip)
		{
			SFXPlayer player = _sfxPool.Get();
			if (player != null)
			{
				player.PlaySFX(clip);
			}
		}

		private void SetupPool()
		{
			Transform sfxPoolParent = new GameObject("===SFX POOL===").transform;
			sfxPoolParent.SetParent(transform);

			_sfxPool = new ObjectPool<SFXPlayer>(
				createFunc: () =>
				{
					SFXPlayer sfxPlayer = Instantiate(_sfxPlayerPrefab, sfxPoolParent);
					sfxPlayer.Initialize(_sfxPool);
					return sfxPlayer;
				},
				actionOnGet: sfxPlayer => sfxPlayer.OnGet(),
				actionOnRelease: sfxPlayer => sfxPlayer.OnRelease(),
				actionOnDestroy: sfxPlayer => sfxPlayer.OnDestroy(),
				defaultCapacity: _maxSfxSources,
				maxSize: _maxSfxSources,
				collectionCheck: false
				);
		}

		private void PreWarmPool()
		{
			if (!_prewarmSfxPool) return;

			var prewarmedPlayers = new List<SFXPlayer>(_maxSfxSources);
			for (int i = 0; i < _maxSfxSources; i++)
			{
				SFXPlayer sfxPlayer = _sfxPool.Get();
				if (sfxPlayer != null)
				{
					prewarmedPlayers.Add(sfxPlayer);
				}
			}
			foreach (SFXPlayer sfxPlayer in prewarmedPlayers)
			{
				_sfxPool.Release(sfxPlayer);
			}
		}

	}

}