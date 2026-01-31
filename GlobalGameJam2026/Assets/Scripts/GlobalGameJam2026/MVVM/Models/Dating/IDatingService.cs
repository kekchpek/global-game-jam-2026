using Cysharp.Threading.Tasks;

namespace GlobalGameJam2026.MVVM.Models.Dating
{
    public interface IDatingService
    {
        UniTask Initialize();
        void SelectAnswer(int optionIndex);
    }
}
