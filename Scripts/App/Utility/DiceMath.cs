using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/14/2018
/// Last Edited By: Jonathon Wigley - 11/14/2018
/// </summary>

namespace VectorStudios.Dice.Math
{
    /// <summary>
    /// Library of math functions that are specifically useful to dice calculations
    /// </summary>
    public static class DiceMath
    {
        /// <summary>
        /// Minimum multiplier for random angular force generation
        /// </summary>
        private static float minAngularForceMultiplier = .075f;

        /// <summary>
        /// Maximum multiplier for random anuglar force generation
        /// </summary>
        private static float maxAngularForceMultiplier = .3f;

        /// <summary>
        /// Returns a rigidbody's velocity with a zero y value
        /// </summary>
        /// <returns>Vector3 velocity with a y value of 0</returns>
        public static Vector3 GetHorizontalVelocity (Rigidbody body)
        {
            return new Vector3 (body.velocity.x, 0, body.velocity.z);
        }

        /// <summary>
        /// Returns a pseduo-random linear force corresponding to the given type of roll
        /// </summary>
        /// <param name="rollType">Type of roll this force is being used for</param>
        /// <returns>Pseudo random linear force for the given roll type</returns>
        public static Vector3 RandomLinearRollForce (RollType rollType)
        {
            GameSettings gameSettings = GameManager.Instance.GameSettings;

            switch (rollType)
            {
                case RollType.Roll:
                    return RandomLinearForce (gameSettings.HorizontalRollForce, gameSettings.VerticalRollForce);

                case RollType.Bounce:
                    return RandomLinearForce (gameSettings.HorizontalBounceForce, gameSettings.VerticalBounceForce);

                case RollType.Nudge:
                    return RandomLinearForce (gameSettings.HorizontalNudgeForce, gameSettings.VerticalNudgeForce);

                default:
                    return RandomLinearRollForce(RollType.Roll);
            }
        }

        /// <summary>
        /// Returns a pseudo-random force to spin the die when it is rolled
        /// </summary>
        /// <returns>Pseudo random angular force for the given roll type</returns>
        public static Vector3 RandomAngularRollForce (RollType rollType)
        {
            GameSettings gameSettings = GameManager.Instance.GameSettings;

            switch (rollType)
            {
                case RollType.Roll:
                    return RandomAngularForce (gameSettings.AngularRollForce, minAngularForceMultiplier, maxAngularForceMultiplier);

                case RollType.Bounce:
                    return RandomAngularForce (gameSettings.AngularBounceForce, minAngularForceMultiplier, maxAngularForceMultiplier);

                case RollType.Nudge:
                    return RandomAngularForce (gameSettings.AngularBounceForce, minAngularForceMultiplier, maxAngularForceMultiplier);

                default:
                    return RandomAngularRollForce(RollType.Roll);
            }
        }

        /// <summary>
        /// Returns a vector in the general direction as the seed velocity
        /// with some randomization and force applied
        /// </summary>
        /// <param name="seedVelocity"></param>
        /// <returns>Angular force</returns>
        public static Vector3 AngularDirectionalForce (Transform objectTransform, Vector3 seedVelocity)
        {
            // Get data used to calculate the normal of a plane perpendicular to the velocity
            Vector3 objectPosition = objectTransform.position;
            Vector3 targetPosition = objectPosition + seedVelocity.normalized;
            Vector3 upPosition = objectPosition + new Vector3 (0, 1, 0);

            // Calculate the normal
            Vector3 objectToUp = upPosition - objectPosition;
            Vector3 objectToTarget = targetPosition - objectPosition;
            Vector3 normal = Vector3.Cross (objectToUp, objectToTarget);

            // Return a vector in the direction of the normal
            return normal + RandomAngularRollForce(RollType.Roll) * .2f;
        }

        /// <summary>
        /// Returns a pseudo-random linear force
        /// </summary>
        /// <param name="horizontalForce">Horizontal force that is multiplied by a random value</param>
        /// <param name="verticalForce">Vertical magnitude of the force</param>
        /// <returns></returns>
        public static Vector3 RandomLinearForce (float horizontalForce, float verticalForce)
        {
            // Generate a random force
            float x = Random.Range (-1f, 1f) * horizontalForce;
            float y = Random.Range (.7f, 1f) * verticalForce;
            float z = Random.Range (-1f, 1f) * horizontalForce;
            Vector3 force = new Vector3 (x, y, z);

            return force;
        }

        /// <summary>
        /// Returns a pseudo random angular force
        /// </summary>
        /// <param name="angularForce">Base force magnitude multiplied by the random number generated</param>
        /// <param name="min">Minimum random number that can be generated</param>
        /// <param name="max">Maximum random number that can be generated</param>
        /// <returns></returns>
        public static Vector3 RandomAngularForce (float angularForce, float min, float max)
        {
            float x, y, z;
            float low, high;

            // Get a random range of magnitudes rotations can be at
            int randomSet = Random.Range (0, 5);
            switch (randomSet)
            {
                case 0:
                    low = 0f;
                    high = .2f;
                break;

                case 1:
                    low = .2f;
                    high = .4f;
                break;

                case 2:
                    low = .4f;
                    high = .6f;
                break;

                case 3:
                    low = .6f;
                    high = .8f;
                break;

                case 4:
                    low = .8f;
                    high = 1f;
                break;

                default:
                    low = 0f;
                    high = 1f;
                break;
            }

            // Constrain the magnitude to the min and max
            float distance = max - min;
            low = min + low * distance;
            high = min + high * distance;

            // Determine at random the direction of the force for each axis
            int isNegative;
            isNegative = Random.Range (0, 2);
            x = Random.Range (low, high);
            if (isNegative == 0) x *= -1;

            isNegative = Random.Range (0, 2);
            y = Random.Range (low, high);
            if (isNegative == 0) y *= -1;

            isNegative = Random.Range (0, 2);
            z = Random.Range (low, high);
            if (isNegative == 0) z *= -1;

            // Return the generated vector
            return new Vector3 (x, y, z) * angularForce;
        }
    }
}