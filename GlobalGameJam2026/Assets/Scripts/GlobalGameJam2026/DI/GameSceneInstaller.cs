using DI.Core;
using GlobalGameJam2026.MVVM.Models.Dating;
using GlobalGameJam2026.MVVM.Views.DatingScreen;
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
            Container.ProvideAccessForViewModelLayer<IDatingModel>();
            Container.ProvideAccessForViewModelLayer<IDatingService>();
            
            Container.InstallView<DatingScreenView, IDatingScreenViewModel, DatingScreenViewModel>(ViewNames.DatingScreen);
        }
    }
}
