using System;

namespace OOPLab.Modules.InclinedPlaneSetup
{
    [Serializable]
    public struct InclinedPlaneTrial
    {
        public int TrialNumber;
        public float AngleDegrees;
        public float FrictionCoefficient;
        public float DistanceMeters;
        public float TimeSeconds;
        public float TwoDistance;
        public float TimeSquared;
        public float CalculatedAcceleration;
    }
}
