using TMPro;
using UnityEngine;
using UnityMVVM;

namespace GlobalGameJam2026.MVVM.Views.DialogueQuestion
{
    public class DialogueQuestionView : ViewBehaviour<IDialogueQuestionViewModel>
    {
        [SerializeField] private TextMeshProUGUI _displayText;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SmartBind(ViewModel.DisplayText, OnDisplayTextChanged);
        }

        private void OnDisplayTextChanged()
        {
            _displayText.text = ViewModel.DisplayText.Value;
        }
    }
}
