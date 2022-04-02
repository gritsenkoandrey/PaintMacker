using UnityEngine;

namespace Utils
{
    public static class Animations
    {
        private const string AnimationRun = "Run";
        private const string AnimationIdle = "Idle";
        private const string AnimationVictory = "Victory";
        private const string AnimationDeath = "Death";
        private const string AnimationVelocity = "Velocity";
        
        public static int Run { get; }
        public static int Idle { get; }
        public static int Victory { get; }
        public static int Death { get; }
        public static int Velocity { get; }

        static Animations()
        {
            Run = Animator.StringToHash(AnimationRun);
            Idle = Animator.StringToHash(AnimationIdle);
            Victory = Animator.StringToHash(AnimationVictory);
            Death = Animator.StringToHash(AnimationDeath);
            Velocity = Animator.StringToHash(AnimationVelocity);
        }
    }
}