using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject
{
    public int boidCount = 5;
    public float spawnRadius = 1f;
    
    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float perceptionRadius = 2.5f;
    public float avoidanceRadius = 1;
    public float maxSteerForce = 3;

    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float separationWeight = 1f;

    public float targetWeight = 1f;

    [Header ("Collisions")]
    public LayerMask obstacleMask;
    public float boundsRadius = .27f;
    public float avoidCollisionWeight = 10;
    public float collisionAvoidDst = 5;

}