using System.Collections.Generic;
using AsyncReactAwait.Bindable;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.RedFlagsIndicator
{
    public interface IRedFlagsIndicatorViewModel : IViewModel
    {
        IBindable<IReadOnlyList<bool>> Steps { get; }
        IBindable<int> TotalStepsCount { get; }
    }
}