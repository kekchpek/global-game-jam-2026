using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GlobalGameJam2026.MVVM.Models.Dating.Data;
using GlobalGameJam2026.MVVM.Views.DialogueOptions.Components;
using kekchpek.Auxiliary;
using UnityEngine;
using UnityMVVM;

namespace GlobalGameJam2026.MVVM.Views.DialogueOptions
{
    public class DialogueOptionsView : ViewBehaviour<IDialogueOptionsViewModel>
    {
        private const string ShowSequence = "Show";
        private const string HideSequence = "Hide";
        
        [SerializeField] private DialogueOptionComponent _optionPrefab;
        [SerializeField] private Transform _optionsContainer;
        [SerializeField] private AnimationController _animationController;
        
        private readonly List<DialogueOptionComponent> _optionComponents = new List<DialogueOptionComponent>();
        
        public event Action<int> OptionSelected;

        /// <summary>
        /// Shows options with animation.
        /// </summary>
        public async UniTask ShowOptions()
        {
            _optionsContainer.gameObject.SetActive(true);
            
            if (_animationController != null && _animationController.HasSequence(ShowSequence))
            {
                await _animationController.PlaySequence(ShowSequence);
            }
        }

        /// <summary>
        /// Hides options with animation.
        /// </summary>
        public async UniTask HideOptions()
        {
            if (_animationController != null && _animationController.HasSequence(HideSequence))
            {
                await _animationController.PlaySequence(HideSequence);
            }
            
            _optionsContainer.gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets the options data and creates UI elements.
        /// </summary>
        public void SetOptions(IReadOnlyList<DialogueOptionData> options)
        {
            ClearOptions();
            CreateOptions(options);
        }

        private void ClearOptions()
        {
            foreach (var option in _optionComponents)
            {
                Destroy(option.gameObject);
            }
            _optionComponents.Clear();
        }

        private void CreateOptions(IReadOnlyList<DialogueOptionData> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                var optionComponent = Instantiate(_optionPrefab, _optionsContainer);
                optionComponent.Setup(i, options[i].QuestionText, OnOptionSelected);
                _optionComponents.Add(optionComponent);
            }
        }

        private void OnOptionSelected(int optionIndex)
        {
            OptionSelected?.Invoke(optionIndex);
        }
    }
}
