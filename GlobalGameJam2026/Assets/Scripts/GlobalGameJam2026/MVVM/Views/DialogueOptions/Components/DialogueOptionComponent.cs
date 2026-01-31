using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlobalGameJam2026.MVVM.Views.DialogueOptions.Components
{
    public class DialogueOptionComponent : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _optionText;

        private int _optionIndex;
        private Action<int> _onSelected;

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        public void Setup(int optionIndex, string text, Action<int> onSelected)
        {
            _optionIndex = optionIndex;
            _optionText.text = text;
            _onSelected = onSelected;
        }

        private void OnButtonClicked()
        {
            _onSelected?.Invoke(_optionIndex);
        }
    }
}
