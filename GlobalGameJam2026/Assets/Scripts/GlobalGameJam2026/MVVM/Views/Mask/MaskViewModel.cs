using AsyncReactAwait.Bindable;
using GlobalGameJam2026.MVVM.Models.Dating;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.Mask
{
    public class MaskViewModel : ViewModel, IMaskViewModel
    {
        private readonly IDatingModel _datingModel;

        public IBindable<int> CurrentMask => _datingModel.CurrentDate;
        public IBindable<bool> GameOver => _datingModel.IsGameOver;

        public MaskViewModel(IDatingModel datingModel)
        {
            _datingModel = datingModel;
        }
    }
}