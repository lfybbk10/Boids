using Boids.Entitas;

public class BoidSimulationSystems : Feature
{
    public BoidSimulationSystems(GameContext context, BoidSettings settings) 
        : base("Boid Simulation Systems")
    {
        Add(new InitializeBoidsSystem(context, settings));
        Add(new InitializeBoidGameObjectsSystem(context));
        
        Add(new CalculateFlockParametersSystem(context, settings));
        Add(new CheckCollisionTrajectorySystem(context, settings));
        Add(new CalculateTargetOffsetSystem(context, settings));
        Add(new CalculateCohesionSystem(context, settings));
        Add(new CalculateSeparationSystem(context, settings));
        Add(new CalculateAlignmentSystem(context, settings));
        Add(new CalculateCollisionAvoidanceSystem(context, settings));
        Add(new CalculateAccelerationSystem(context));
        Add(new CalculateVelocitySystem(context, settings));
        
        Add(new UpdatePositionSystem(context));
        Add(new UpdateRotationSystem(context));
        Add(new UpdateLinkedGameObjectPositionSystem(context));
    }
    
}
