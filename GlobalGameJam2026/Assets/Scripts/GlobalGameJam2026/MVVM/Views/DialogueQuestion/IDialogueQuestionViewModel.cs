using AsyncReactAwait.Bindable;
using GlobalGameJam2026.MVVM.Views.DatingScreen;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.DialogueQuestion
{
    public interface IDialogueQuestionViewModel : IViewModel
    {
        IBindable<string> DisplayText { get; }
    }
}
