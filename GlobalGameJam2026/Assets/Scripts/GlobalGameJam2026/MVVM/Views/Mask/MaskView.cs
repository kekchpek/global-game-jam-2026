using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityMVVM;

namespace GlobalGameJam2026.MVVM.Views.Mask
{
    public class MaskView : ViewBehaviour<IMaskViewModel>
    {
        private const string MaskSkinPrefix = "Mask_";
        private const string DefaultMask = "Mask_1";

        [SerializeField]
        private SkeletonGraphic _skeletonGraphic;

        private readonly HashSet<string> _activeMasks = new();

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            
            InitializeMasks(ViewModel.CurrentMask.Value);
            
            SmartBind(ViewModel.GameOver, RemoveMasks);
        }

        private void InitializeMasks(int currentMask)
        {
            _activeMasks.Clear();
            
            if (currentMask <= 1)
            {
                _activeMasks.Add(DefaultMask);
            }
            else
            {
                for (int i = 2; i <= currentMask; i++)
                {
                    _activeMasks.Add(MaskSkinPrefix + i);
                }
            }
            
            UpdateSkin();
        }

        private void RemoveMasks(bool over)
        {
            if (over)
            {
                _activeMasks.Clear();
                _activeMasks.Add(DefaultMask);
                UpdateSkin();
            }

        }

        private void UpdateSkin()
        {
            var combinedSkin = new Skin("combinedSkin");
            
            foreach (var maskName in _activeMasks)
            {
                var maskData = _skeletonGraphic.SkeletonData.FindSkin(maskName);
                if (maskData != null)
                {
                    combinedSkin.AddSkin(maskData);
                }
                else 
                {
                    Debug.LogError($"MaskSkinCombiner: Skin '{maskName}' not found in skeleton data on {gameObject.name}");
                }
            }
            
            _skeletonGraphic.Skeleton.SetSkin(combinedSkin);
            _skeletonGraphic.Skeleton.SetSlotsToSetupPose();
        }
    }
}