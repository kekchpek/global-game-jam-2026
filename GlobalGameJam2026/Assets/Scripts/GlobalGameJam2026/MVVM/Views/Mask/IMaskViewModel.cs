using AsyncReactAwait.Bindable;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.Mask
{
    public interface IMaskViewModel : IViewModel
    {
        IBindable<int> CurrentMask { get; }
        IBindable<bool> GameOver { get; }
    }
}