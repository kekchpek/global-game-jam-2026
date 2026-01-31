using System.Collections.Generic;
using Newtonsoft.Json;

namespace GlobalGameJam2026.MVVM.Models.Dating.Data
{
    public class DialogueQuestionData
    {
        [JsonProperty]
        private string id;

        [JsonProperty]
        private string question;

        [JsonProperty]
        private List<DialogueOptionData> options;

        [JsonProperty]
        private List<string> winDialogues;

        [JsonProperty]
        private List<string> loseDialogues;

        public string Id => id;
        public string Question => question;
        public IReadOnlyList<DialogueOptionData> Options => options;
        public IReadOnlyList<string> WinDialogues => winDialogues;
        public IReadOnlyList<string> LoseDialogues => loseDialogues;
    }
}
