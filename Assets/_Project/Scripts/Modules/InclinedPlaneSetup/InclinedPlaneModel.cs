using UnityEngine;

namespace OOPLab.Modules.InclinedPlaneSetup
{
    [System.Serializable]
    public class InclinedPlaneModel
    {
        public float AngleDegrees { get; private set; } = 25f;
        public float FrictionCoefficient { get; private set; } = 0.12f;
        public float RampLengthMeters { get; private set; } = 5f;
        public float TravelDistanceMeters { get; private set; } = 2f;

        private const float Gravity = 9.81f;

        public void SetAngle(float angleDegrees)
        {
            AngleDegrees = Mathf.Clamp(angleDegrees, 0f, 45f);
        }

        public void SetFriction(float frictionCoefficient)
        {
            FrictionCoefficient = Mathf.Clamp(frictionCoefficient, 0f, 0.8f);
        }

        public void SetRampLength(float rampLengthMeters)
        {
            RampLengthMeters = Mathf.Max(0.1f, rampLengthMeters);
        }

        public void SetTravelDistance(float distanceMeters)
        {
            TravelDistanceMeters = Mathf.Clamp(distanceMeters, 0.5f, 4.5f);
        }

        public float CalculateAcceleration()
        {
            float angleRadians = AngleDegrees * Mathf.Deg2Rad;
            float downhillForce = Gravity * Mathf.Sin(angleRadians);
            float frictionForce = FrictionCoefficient * Gravity * Mathf.Cos(angleRadians);
            return Mathf.Max(0f, downhillForce - frictionForce);
        }
    }
}
