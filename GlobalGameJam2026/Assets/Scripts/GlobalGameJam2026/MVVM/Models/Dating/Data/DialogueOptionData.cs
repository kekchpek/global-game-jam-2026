using Newtonsoft.Json;

namespace GlobalGameJam2026.MVVM.Models.Dating.Data
{
    public class DialogueOptionData
    {
        [JsonProperty]
        private string text;

        [JsonProperty]
        private bool isCorrect;

        public string Text => text;
        public bool IsCorrect => isCorrect;
    }
}
