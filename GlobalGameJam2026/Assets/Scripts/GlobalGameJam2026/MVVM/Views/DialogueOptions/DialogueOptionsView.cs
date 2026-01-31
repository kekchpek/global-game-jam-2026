using System.Collections.Generic;
using GlobalGameJam2026.MVVM.Models.Dating.Data;
using GlobalGameJam2026.MVVM.Views.DialogueOptions.Components;
using UnityEngine;
using UnityMVVM;

namespace GlobalGameJam2026.MVVM.Views.DialogueOptions
{
    public class DialogueOptionsView : ViewBehaviour<IDialogueOptionsViewModel>
    {
        [SerializeField] private DialogueOptionComponent _optionPrefab;
        [SerializeField] private Transform _optionsContainer;
        
        private readonly List<DialogueOptionComponent> _optionComponents = new List<DialogueOptionComponent>();

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SmartBind(ViewModel.Options, OnOptionsChanged);
        }

        private void OnOptionsChanged()
        {
            ClearOptions();
            CreateOptions(ViewModel.Options.Value);
            if (ViewModel.Options.Value.Count == 0)
            {
                _optionsContainer.gameObject.SetActive(false);
            }
            else
            {
                _optionsContainer.gameObject.SetActive(true);
            }
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
            ViewModel.SelectOption(optionIndex);
        }
    }
}
