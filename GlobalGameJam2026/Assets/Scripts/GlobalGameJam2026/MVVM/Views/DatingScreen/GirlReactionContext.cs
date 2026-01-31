namespace GlobalGameJam2026.MVVM.Views.DatingScreen
{
    public readonly struct GirlReactionContext
    {
        public GirlReaction Reaction { get; }
        public string ReactionText { get; }

        public GirlReactionContext(GirlReaction reaction, string reactionText)
        {
            Reaction = reaction;
            ReactionText = reactionText;
        }

        public static GirlReactionContext None => new GirlReactionContext(GirlReaction.None, string.Empty);
    }
}
