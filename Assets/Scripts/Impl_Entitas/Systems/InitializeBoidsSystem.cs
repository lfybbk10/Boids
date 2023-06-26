using Entitas;
using Unity.Mathematics;

namespace Boids.Entitas
{
    public class InitializeBoidsSystem : IInitializeSystem
    {
        private readonly GameContext _context;

        private readonly int _boidCount;
        private readonly float _spawnRadius;
        private readonly float _minSpeed;
        private readonly float _maxSpeed;

        public InitializeBoidsSystem(GameContext context, BoidSettings settings)
        {
            _context = context;

            _boidCount = settings.boidCount;
            _spawnRadius = settings.spawnRadius;
            _minSpeed = settings.minSpeed;
            _maxSpeed = settings.maxSpeed;
        }

        public void Initialize()
        {
            for (int i = 0; i < _boidCount; i++)
            {
                GameEntity e = _context.CreateEntity();

                float3 position = BoidsMathUtility.InsideUnitSphere() * _spawnRadius;
                e.AddPosition(position);

                float3 rotation = math.forward(quaternion.Euler(BoidsMathUtility.InsideUnitSphere()));
                e.AddRotation(rotation);

                float startSpeed = (_minSpeed + _maxSpeed) / 2;
                e.AddVelocity(rotation * startSpeed);

                e.AddAcceleration(float3.zero);
                e.AddFollowingTarget(float3.zero);

                e.AddFlockmateNumber(0);
                e.AddFlockCenter(float3.zero);
                e.AddCohesion(float3.zero);

                e.AddAverageAvoidance(float3.zero);
                e.AddSeparation(float3.zero);

                e.AddAverageFlockDirection(float3.zero);
                e.AddAlignment(float3.zero);

                e.AddTargetOffset(float3.zero);

                e.AddIsOnCollisionTrajectory(false);
                e.AddAvoidance(float3.zero);

                e.AddLinkedGO(null);
            }
        }
    }
}