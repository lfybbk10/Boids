using UnityEngine;

public class EntitasGameController : MonoBehaviour
{
    [SerializeField] private BoidSettings boidSettings = null;
        
    private BoidSimulationSystems _systems;

    private void Start()
    {
        Contexts contexts = Contexts.sharedInstance;

        _systems = new BoidSimulationSystems(contexts.game, boidSettings);

        _systems.Initialize();
    }
        
    private void Update()
    {
        _systems.Execute();
        _systems.Cleanup();
    }
}