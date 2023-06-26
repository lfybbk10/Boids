using UnityEngine;

public class BoidManager : MonoBehaviour 
{
    [SerializeField] private BoidSettings settings = null;
    [SerializeField] private Transform boidTarget = null;
    
    public ComputeShader compute;

    private Boid_SL[] boids;

    private void Start ()
    {
        boids = FindObjectsOfType<Boid_SL> ();

        for (int i = 0; i < boids.Length; i++)
        {
            Boid_SL boid = boids[i];
            boid.Initialize(settings, boidTarget);
        }
    }

    private void Update ()
    {
        for (int i = 0; i < boids.Length; i++)
        {
            Vector3 flockHeading = Vector3.zero;
            Vector3 flockCenter = Vector3.zero;
            Vector3 avoidanceHeading = Vector3.zero;
            int numFlockmates = 0;
            
            for (int j = 0; j < boids.Length; j++)
            {
                if (i != j)
                {
                    Vector3 dist = boids[j].boidPosition - boids[i].boidPosition;

                    if (dist.magnitude < settings.perceptionRadius)
                    {
                        numFlockmates++;
                        flockHeading += boids[j].forward;
                        flockCenter += boids[j].boidPosition;

                        if (dist.magnitude < settings.avoidanceRadius)
                        {
                            avoidanceHeading -= dist / dist.sqrMagnitude;
                        }
                    }
                }
            }
            
            boids[i].avgFlockHeading = flockHeading;
            boids[i].centreOfFlockmates = flockCenter;
            boids[i].avgAvoidanceHeading = avoidanceHeading;
            boids[i].numPerceivedFlockmates = numFlockmates;

            boids[i].UpdateBoid();
        }

        /*int numBoids = boids.Length;
        BoidData[] boidData = new BoidData[numBoids];

        for (int i = 0; i < boids.Length; i++) 
        {
            boidData[i].position = boids[i].boidPosition;
            boidData[i].direction = boids[i].forward;
        }
        
        ComputeBuffer boidBuffer = new ComputeBuffer (numBoids, BoidData.Size);
        boidBuffer.SetData (boidData);

        compute.SetBuffer (0, "boids", boidBuffer);
        compute.SetInt ("numBoids", boids.Length);
        compute.SetFloat ("viewRadius", settings.perceptionRadius);
        compute.SetFloat ("avoidRadius", settings.avoidanceRadius);

        int threadGroups = Mathf.CeilToInt (numBoids / (float) 1024);
        compute.Dispatch (0, threadGroups, 1, 1);

        boidBuffer.GetData (boidData);

        
        for (int i = 0; i < boids.Length; i++) 
        {
            boids[i].avgFlockHeading = boidData[i].flockHeading;
            boids[i].centreOfFlockmates = boidData[i].flockCentre;
            boids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
            boids[i].numPerceivedFlockmates = boidData[i].numFlockmates;

            boids[i].UpdateBoid ();
        }
        
        boidBuffer.Release();*/
    }

    public struct BoidData {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCentre;
        public Vector3 avoidanceHeading;
        public int numFlockmates;

        public static int Size {
            get {
                return sizeof (float) * 3 * 5 + sizeof (int);
            }
        }
    }
}