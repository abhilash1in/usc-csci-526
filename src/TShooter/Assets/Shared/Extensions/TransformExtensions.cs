using UnityEngine;

namespace Shared.Extensions
{
    public static class TransformExtensions
    {

        /// <summary>
        /// Check if the target is within line of sight
        /// </summary>
        /// <returns><c>true</c>, if in line of sight was ised, <c>false</c> otherwise.</returns>
        /// <param name="origin">Transform origin.</param>
        /// <param name="target">Target direction.</param>
        /// <param name="fieldOfView">Field of view.</param>
        /// <param name="collisionMask">Check against these layers.</param>
        /// <param name="offset">Transform's origin offset.</param>
        /// <returns>Yes or No.</returns>
        public static bool IsInLineOfSight(this Transform origin, Vector3 target, float fieldOfView, LayerMask collisionMask, Vector3 offset)
        {
            Vector3 direction = target - origin.position;
            if (Vector3.Angle(origin.forward, direction.normalized) < (fieldOfView / 2))
            {
                float distanceToTarget = Vector3.Distance(origin.position, target);

                // Something blocking our view
                if (Physics.Raycast(origin.position + offset + origin.forward * 0.3f, direction.normalized, distanceToTarget, collisionMask))
                    return false;

                return true;
            }
            return false;
        }
    }
}

