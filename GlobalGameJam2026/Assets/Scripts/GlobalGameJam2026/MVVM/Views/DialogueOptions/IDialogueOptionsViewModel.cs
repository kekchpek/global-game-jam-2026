using System.Collections.Generic;
using AsyncReactAwait.Bindable;
using GlobalGameJam2026.MVVM.Models.Dating.Data;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.DialogueOptions
{
    public interface IDialogueOptionsViewModel : IViewModel
    {
        IBindable<IReadOnlyList<DialogueOptionData>> Options { get; }
        
        void SelectOption(int optionIndex);
    }
}
