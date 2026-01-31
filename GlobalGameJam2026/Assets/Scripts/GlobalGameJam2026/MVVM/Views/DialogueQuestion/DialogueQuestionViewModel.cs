using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using GlobalGameJam2026.MVVM.Models.Dating;
using GlobalGameJam2026.MVVM.Models.Dating.Data;
using GlobalGameJam2026.MVVM.Views.DatingScreen;
using kekchpek.Auxiliary.Contexts;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.DialogueQuestion
{
    public class DialogueQuestionViewModel : ViewModel, 
        IDialogueQuestionViewModel, 
        IContextedViewModel<GirlReactionContext>
    {
        private readonly IDatingModel _datingModel;
        private readonly IMutable<string> _displayText = new Mutable<string>(string.Empty);

        public IBindable<string> DisplayText => _displayText;

        public DialogueQuestionViewModel(IDatingModel datingModel)
        {
            _datingModel = datingModel;
            _datingModel.CurrentQuestion.Bind(OnCurrentQuestionChanged);
        }

        private void OnCurrentQuestionChanged(DialogueQuestionData question)
        {
            _displayText.Set(question?.Question ?? string.Empty);
        }

        public void SetContext(GirlReactionContext context)
        {
            if (context.Reaction != GirlReaction.None && !string.IsNullOrEmpty(context.ReactionText))
            {
                _displayText.Set(context.ReactionText);
            }
        }

        protected override void OnDestroyInternal()
        {
            base.OnDestroyInternal();
            _datingModel.CurrentQuestion.Unbind(OnCurrentQuestionChanged);
        }
    }
}
