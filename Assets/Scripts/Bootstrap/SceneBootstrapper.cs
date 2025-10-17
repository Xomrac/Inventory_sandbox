
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;

namespace XomracCore.Bootstrap
{
    using AYellowpaper;
    using UnityEngine;

    // component to handle a single entry point for bootstrapping all services in a scene
    // this way we can control the order of bootstrapping via the inspector and avoid race conditions
    public class SceneBootstrapper : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IBootstrappable, MonoBehaviour>[] _bootstrappers;
	
        private CancellationTokenSource _cancellationTokenSource;
    
        private void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void Start()
        {
            _ = Bootstrap();
        }

        private async UniTask Bootstrap()
        {
            foreach (InterfaceReference<IBootstrappable, MonoBehaviour> bootstrappable in _bootstrappers)
            {
                try
                {
                    bootstrappable.Value.PrintStartingMessage();
                    await bootstrappable.Value.Bootstrap();
                    bootstrappable.Value.PrintSuccessMessage();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }

            await UniTask.Yield();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
