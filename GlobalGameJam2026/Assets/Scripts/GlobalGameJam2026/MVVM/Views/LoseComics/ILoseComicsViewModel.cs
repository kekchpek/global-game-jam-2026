using UnityMVVM.ViewModelCore;
using AsyncReactAwait.Bindable;

namespace GlobalGameJam2026.MVVM.Views.LoseComics
{
    public interface ILoseComicsViewModel : IViewModel
    {
        void OnAnimationComplete();
        IBindable<int> CurrentMask { get; }
    }
}
