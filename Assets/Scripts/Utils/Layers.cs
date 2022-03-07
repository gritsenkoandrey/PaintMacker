using UnityEngine;

namespace Utils
{
    public static class Layers
    {
        private const string LayerGround = "Ground";
        private const string LayerCharacter = "Character";
        private const string LayerWalking = "Walking";
        private const string LayerDeactivate = "Deactivate";
        private const string LayerFrame = "Frame";
        private const string LayerTrap = "Trap";
        private const string LayerPath = "Path";
        private const string LayerAgent = "Agent";

        public static int Ground { get; }
        public static int Character { get; }
        public static int Walking { get; }
        public static int Deactivate { get; }
        public static int Frame { get; }
        public static int Trap { get; }
        public static int Path { get; }
        public static int Agent { get; }
        public static int GetWalking { get; }
        public static int GetGround { get; }
        public static int GetDeactivate { get; }
        public static int GetPath { get; }

        static Layers()
        {
            Ground = LayerMask.NameToLayer(LayerGround);
            Character = LayerMask.NameToLayer(LayerCharacter);
            Walking = LayerMask.NameToLayer(LayerWalking);
            Deactivate = LayerMask.NameToLayer(LayerDeactivate);
            Frame = LayerMask.NameToLayer(LayerFrame);
            Trap = LayerMask.NameToLayer(LayerTrap);
            Path = LayerMask.NameToLayer(LayerPath);
            Agent = LayerMask.NameToLayer(LayerAgent);
            
            GetWalking = LayerMask.GetMask(LayerGround, LayerWalking, LayerFrame, LayerPath);
            GetGround = LayerMask.GetMask(LayerGround);
            GetDeactivate = LayerMask.GetMask(LayerDeactivate);
            GetPath = LayerMask.GetMask(LayerPath);
        }
    }
}