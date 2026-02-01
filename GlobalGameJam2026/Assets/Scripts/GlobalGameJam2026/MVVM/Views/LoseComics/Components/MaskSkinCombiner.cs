using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace GlobalGameJam2026.MVVM.Views.LoseComics.Components
{
    public class MaskSkinCombiner : MonoBehaviour
    {
        private const string MaskSkin = "mask";

        [SerializeField]
        private SkeletonGraphic _skeletonAnimation;

        [SerializeField]
        private readonly HashSet<string> _masks = new();

        private int _maskCount = 1;

        public void AddMask()
        {
            _maskCount++;
            _masks.Add(MaskSkin + "_" + _maskCount);
            UpdateSkin();
        }

        public void RemoveAllMasks()
        {
            for (int i = 0; i < _maskCount; i++)
            {
                _masks.Remove(MaskSkin + "_" + i);
            }
            _maskCount = 1;
            UpdateSkin();
        }

        private void UpdateSkin()
        {
            var currentSkin = new Skin("combineSkin");
            foreach (var mask in _masks)
            {
                var maskData = _skeletonAnimation.SkeletonData.FindSkin(mask);
                if (maskData != null)
                {
                    currentSkin.AddSkin(maskData);
                }
                else 
                {
                    Debug.LogError($"SpineSkinsCombiner: Skin '{mask}' not found in skeleton data on {gameObject.name}");
                }
            }
            _skeletonAnimation.Skeleton.SetSkin(currentSkin);
            _skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        }
    }
}