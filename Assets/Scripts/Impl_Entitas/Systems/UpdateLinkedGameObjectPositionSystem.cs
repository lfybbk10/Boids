using Entitas;

namespace Boids.Entitas
{
    public class UpdateLinkedGameObjectPositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;

        public UpdateLinkedGameObjectPositionSystem(GameContext context)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.LinkedGO,
                GameMatcher.Position,
                GameMatcher.Rotation
            ));
        }

        public void Execute()
        {
            foreach (GameEntity e in _boidsGroup)
            {
                e.linkedGO.linkedGO.transform.position = e.position.value;
                e.linkedGO.linkedGO.transform.forward = e.rotation.value;
            }
        }
    }
}