using AssetsSystem;
using Cysharp.Threading.Tasks;
using GlobalGameJam2026.Static;
using Startup.Core;
using UnityEngine;
using UnityMVVM.ViewManager;

namespace GlobalGameJam2026
{
    public class GameSceneStartupService : BaseSceneStartupService
    {

        private readonly IViewManager _viewManager;
        private readonly IAssetsModel _assetsModel;

        public GameSceneStartupService(
            IProjectStartupService projectStartupService
            , IViewManager viewManager
            , IAssetsModel assetsModel
            ) : base(projectStartupService)
        {
            _viewManager = viewManager;
            _assetsModel = assetsModel;
        }

        protected override async UniTask SceneStartup()
        {
            await _assetsModel.LoadAsset<GameObject>(ViewNames.DatingScreen);
            await _viewManager.Open(LayerNames.Screen, ViewNames.DatingScreen);
        }
    }
}
