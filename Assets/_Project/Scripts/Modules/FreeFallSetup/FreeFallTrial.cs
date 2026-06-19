using System;

namespace OOPLab.Modules.FreeFallSetup
{
    [Serializable]
    public struct FreeFallTrial
    {
        public int TrialNumber;
        public float HeightMeters;
        public float TimeSeconds;
        public float TwoHeight;
        public float TimeSquared;
        public float CalculatedGravity;
    }
}
