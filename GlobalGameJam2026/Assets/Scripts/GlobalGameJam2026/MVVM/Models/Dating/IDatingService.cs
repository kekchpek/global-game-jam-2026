using Cysharp.Threading.Tasks;
using GlobalGameJam2026.MVVM.Models.Dating.Data;

namespace GlobalGameJam2026.MVVM.Models.Dating
{
    public interface IDatingService
    {
        UniTask Initialize();
        bool SelectAnswer(int optionIndex);
        void SelectNextQuestion();
    }
}
