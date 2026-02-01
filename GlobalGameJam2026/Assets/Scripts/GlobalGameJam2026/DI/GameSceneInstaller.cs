using DI.Core;
using GlobalGameJam2026.MVVM.Models.Dating;
using GlobalGameJam2026.MVVM.Views.DatingScreen;
using GlobalGameJam2026.MVVM.Views.DialogueOptions;
using GlobalGameJam2026.MVVM.Views.DialogueQuestion;
using GlobalGameJam2026.MVVM.Views.LoseComics;
using GlobalGameJam2026.MVVM.Views.Mask;
using GlobalGameJam2026.MVVM.Views.LoseScreen;
using GlobalGameJam2026.MVVM.Views.RedFlagsIndicator;
using GlobalGameJam2026.MVVM.Views.WinScreen;
using GlobalGameJam2026.Static;
using Startup.Core;
using UnityMVVM.DI;
using Zenject;

namespace GlobalGameJam2026
{
    public class GameSceneInstaller : MVVMInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<IStartupService>().To<GameSceneStartupService>().AsSingle().WhenInjectedInto<StartupBehaviour>();
            
            Container.Install<DatingInstaller>();
            
            Container.InstallView<DatingScreenView, IDatingScreenViewModel, DatingScreenViewModel>(ViewNames.DatingScreen);
            Container.InstallView<DialogueOptionsView, IDialogueOptionsViewModel, DialogueOptionsViewModel>();
            Container.InstallView<DialogueQuestionView, IDialogueQuestionViewModel, DialogueQuestionViewModel>();
            Container.InstallView<RedFlagsIndicatorView, IRedFlagsIndicatorViewModel, RedFlagsIndicatorViewModel>();
            Container.InstallView<LoseComicsView, ILoseComicsViewModel, LoseComicsViewModel>(ViewNames.LoseComics);
            Container.InstallView<MaskView, IMaskViewModel, MaskViewModel>();
            Container.InstallView<WinScreenView, IWinScreenViewModel, WinScreenViewModel>(ViewNames.WinScreen);
            Container.InstallView<LoseScreenView, ILoseScreenViewModel, LoseScreenViewModel>(ViewNames.LoseScreen);
        }
    }
}
