namespace InventorySandbox.InventorySystem
{

	using System.Collections;
	using Items;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Panel that displays notifications when items are added to the inventory.
	/// Slides in and out from the top of the screen with an eased animation.
	/// </summary>
	public class NotificationDisplayer : MonoBehaviour
	{
		[SerializeField] private CharacterInventory _inventoryToTrack;
		[SerializeField] private RectTransform _panel;
		[SerializeField] private Image _icon;
		[SerializeField] private TextMeshProUGUI _textLabel;
		[Header(" Animation Settings")] [SerializeField] private float _slideDuration = 0.5f;
		[SerializeField] private float _displayDuration = 2f;
		[SerializeField] private AnimationCurve _slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		private float _panelHeight;

		private void Awake()
		{
			_panelHeight = _panel.rect.height;
			_panel.anchoredPosition = new Vector2(_panel.anchoredPosition.x, _panelHeight);
			_panel.gameObject.SetActive(false);
		}

		private void Start()
		{
			if (_inventoryToTrack != null)
			{
				_inventoryToTrack.Inventory.ItemAdded += ShowNotification;
			}
		}

		private void ShowNotification(Item item)
		{
			_icon.sprite = item.Icon;
			_textLabel.text = item.Name;
			_textLabel.color = item.Tier.Color;
			_panel.gameObject.SetActive(true);
			StopAllCoroutines();
			StartCoroutine(Display());
		}

		private IEnumerator Display()
		{
			var startPos = new Vector2(_panel.anchoredPosition.x, _panelHeight);
			var endPos = new Vector2(_panel.anchoredPosition.x, 0);

			yield return SlidePanel(startPos, endPos);
			yield return new WaitForSeconds(_displayDuration);
			yield return SlidePanel(endPos, startPos);

			_panel.gameObject.SetActive(false);
		}

		private IEnumerator SlidePanel(Vector2 from, Vector2 to)
		{
			float elapsed = 0f;
			while (elapsed < _slideDuration)
			{
				elapsed += Time.deltaTime;
				float t = _slideCurve.Evaluate(elapsed / _slideDuration);
				_panel.anchoredPosition = Vector2.Lerp(from, to, t);
				yield return null;
			}
			_panel.anchoredPosition = to;
		}

	}

}