using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public static class BoidsMathUtility
{
    private static Unity.Mathematics.Random rng;

    static BoidsMathUtility()
    {
        rng = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(0, int.MaxValue));
    }
    
    public static float3 InsideUnitSphere() => math.normalize(new float3(rng.NextFloat(-1f, 1f),
        rng.NextFloat(-1f, 1f), rng.NextFloat(-1f, 1f)));
    
    public static float3 GetClampedDirection(float3 vector, float3 velocity, float maxSpeed, float maxSteerForce)
    {
        float3 v = math.normalizesafe(vector) * maxSpeed - velocity;
        return ClampMagnitude(v, maxSteerForce);
    }

    public static float3 ClampMagnitude(float3 vector, float maxLength)
    {
        float sqrMagnitude = math.lengthsq(vector);
        if (sqrMagnitude <= maxLength * maxLength)
            return vector;

        float magnitude = math.sqrt(sqrMagnitude);
        float3 normalizedVector = vector / magnitude;
        return normalizedVector * maxLength;
    }
    
    public static float3[] GetAvoidanceRayDirections(int avoidanceRayDirectionCount)
    {
        float3[] directions = new float3[avoidanceRayDirectionCount];

        float goldenRatio = (1 + math.sqrt (5)) / 2;
        float angleIncrement = math.PI * 2 * goldenRatio;

        for (int i = 0; i < avoidanceRayDirectionCount; i++)
        {
            float t = (float) i / avoidanceRayDirectionCount;
            float inclination = math.acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = math.sin(inclination) * math.cos(azimuth);
            float y = math.sin(inclination) * math.sin(azimuth);
            float z = math.cos(inclination);
            directions[i] = new float3(x, y, z);
        }

        return directions;
    }
    
    public static float3 GetClosestPointOnRay(float3 origin, float3 direction, float3 point)
    {
        float3 rayOriginToPoint = point - origin;
        float projectionLength = math.dot(rayOriginToPoint, direction);
        float3 closestPointOnRay = origin + projectionLength * direction;

        //float3 closestPointToB = point - closestPointOnRay;
        //float distance = math.length(closestPointToB);

        return closestPointOnRay;
    }
}
