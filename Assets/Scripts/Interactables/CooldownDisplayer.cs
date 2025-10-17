using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Diablo5
{

    public class CooldownDisplayer : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        
        private Timer _cooldown;
        public void DisplayCooldown(Timer cooldown)
        {
            if (cooldown == null) return;
            _cooldown = cooldown;
            _fillImage.fillAmount = _cooldown.Progress;
            gameObject.SetActive(true);
            _cooldown.Updated += OnUpdated;
        }

        private void OnUpdated(float progress)
        {
            _fillImage.fillAmount = progress;
            if (progress >= 1f)
            {
                gameObject.SetActive(false);
                _cooldown.Updated -= OnUpdated;
                _cooldown = null;
            }
            
        }
    }
}
