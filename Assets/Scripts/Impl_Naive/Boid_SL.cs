using UnityEngine;

public class Boid_SL : MonoBehaviour 
{
    private BoidSettings settings;
    
    [HideInInspector] public Vector3 boidPosition;
    [HideInInspector] public Vector3 forward;
    private Vector3 velocity;
    
    [HideInInspector] public Vector3 avgFlockHeading;
    [HideInInspector] public Vector3 avgAvoidanceHeading;
    [HideInInspector] public Vector3 centreOfFlockmates;
    [HideInInspector] public int numPerceivedFlockmates;
    
    private Transform target;
    
    private bool IsHeadingForCollision => 
        Physics.SphereCast(boidPosition, settings.boundsRadius, forward, out _,
            settings.collisionAvoidDst, settings.obstacleMask);
    
    public void Initialize (BoidSettings settings, Transform target)
    {
        this.target = target;
        this.settings = settings;

        boidPosition = transform.position;
        forward = transform.forward;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }
    
    public void UpdateBoid () 
    {
        Vector3 acceleration = Vector3.zero;

        if (target != null) 
        {
            Vector3 offsetToTarget = target.position - boidPosition;
            acceleration = BoidHelper.SteerTowards(offsetToTarget, velocity, 
                settings.maxSpeed, settings.maxSteerForce) * settings.targetWeight;
        }

        if (numPerceivedFlockmates != 0) 
        {
            centreOfFlockmates /= numPerceivedFlockmates;

            Vector3 offsetToFlockmatesCentre = centreOfFlockmates - boidPosition;

            Vector3 alignmentForce = BoidHelper.SteerTowards(avgFlockHeading, velocity, 
                settings.maxSpeed, settings.maxSteerForce) * settings.alignmentWeight;
            Vector3 cohesionForce = BoidHelper.SteerTowards(offsetToFlockmatesCentre, velocity, 
                settings.maxSpeed, settings.maxSteerForce) * settings.cohesionWeight;
            Vector3 separationForce = BoidHelper.SteerTowards(avgAvoidanceHeading, velocity, 
                settings.maxSpeed, settings.maxSteerForce) * settings.separationWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += separationForce;
        }

        if (IsHeadingForCollision) 
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = BoidHelper.SteerTowards(collisionAvoidDir, velocity, 
                settings.maxSpeed, settings.maxSteerForce) * settings.avoidCollisionWeight;
            acceleration += collisionAvoidForce;
        }

        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        velocity = dir * speed;

        transform.position += velocity * Time.deltaTime;
        transform.forward = dir;
        boidPosition = transform.position;
        forward = dir;
    }
    
    private Vector3 ObstacleRays() 
    {
        Vector3[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++) 
        {
            Vector3 dir = transform.TransformDirection(rayDirections[i]);
            Ray ray = new(boidPosition, dir);
            
            if (!Physics.SphereCast(ray, settings.boundsRadius, 
                    settings.collisionAvoidDst, settings.obstacleMask))
            {
                return dir;
            }
        }

        return forward;
    }
}