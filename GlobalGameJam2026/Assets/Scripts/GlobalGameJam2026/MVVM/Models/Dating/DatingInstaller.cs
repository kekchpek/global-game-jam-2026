using UnityMVVM.DI;
using Zenject;

namespace GlobalGameJam2026.MVVM.Models.Dating
{
    public class DatingInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.FastBind<IDatingMutableModel, IDatingModel, DatingModel>();
            Container.FastBind<IDatingService, DatingService>();
        }
    }
}
