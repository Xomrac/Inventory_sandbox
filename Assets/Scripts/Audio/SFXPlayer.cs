using UnityEngine.Audio;

namespace XomracCore.Audio
{

	using System.Collections;
	using UnityEngine;
	using UnityEngine.Pool;

	public class SFXPlayer : MonoBehaviour
	{
		private AudioSource _audioSource;
		private ObjectPool<SFXPlayer> _pool;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
			if (_audioSource == null)
			{
				Debug.LogError("SFXPlayer requires an AudioSource component.");
			}
		}

		public void Initialize(ObjectPool<SFXPlayer> pool)
		{
			_pool = pool;
		}

		public void PlaySFX(AudioClip clip)
		{
			_audioSource.PlayOneShot(clip);
			StartCoroutine(WaitAndRelease());
		}
		public void PlaySFX(AudioResource audioResource)
		{
			_audioSource.resource = audioResource;
			_audioSource.Play();
			StartCoroutine(WaitAndRelease());
		}

		private IEnumerator WaitAndRelease()
		{
			yield return new WaitUntil(() => !_audioSource.isPlaying);
			_pool.Release(this);
		}
		
		//Pooling system implementations

		public void Initialize<T>(ObjectPool<T> pool) where T : MonoBehaviour
		{
			_pool = pool as ObjectPool<SFXPlayer>;
		}

		public void OnGet()
		{
			gameObject.SetActive(true);
		}

		public void OnRelease()
		{
			_audioSource.Stop();
			gameObject.SetActive(false);
		}

		public void OnDestroy()
		{
			Destroy(gameObject);
		}
	}

}