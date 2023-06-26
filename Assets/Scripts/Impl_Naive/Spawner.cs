using UnityEngine;

public class Spawner : MonoBehaviour 
{
    public Boid_SL prefab;
    public BoidSettings boidSettings = null;

    private void Awake () 
    {
        for (int i = 0; i < boidSettings.boidCount; i++) 
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * boidSettings.spawnRadius;
            Boid_SL boidSl = Instantiate (prefab);
            boidSl.transform.position = pos;
            boidSl.transform.forward = Random.insideUnitSphere;
        }
    }
}