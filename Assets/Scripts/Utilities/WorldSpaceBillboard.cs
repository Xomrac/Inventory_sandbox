using UnityEngine;

namespace Diablo5
{
    public class WorldSpaceBillboard : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Start()
        {
            if (Camera.main != null)
            {
                _cameraTransform = Camera.main.transform;
            }
        }

        private void LateUpdate()
        {
            if (_cameraTransform != null)
            {
                transform.LookAt(transform.position + _cameraTransform.rotation * Vector3.forward,
                    _cameraTransform.rotation * Vector3.up);
            }
        }
    }
}
