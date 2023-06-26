using Entitas;
using UnityEngine;

namespace Boids.Entitas
{
    public class InitializeBoidGameObjectsSystem : IInitializeSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;
        private readonly GameObject parentGO;

        public InitializeBoidGameObjectsSystem(GameContext context)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.LinkedGO));

            parentGO = new GameObject("Boid_LinkedGO_Parent");
        }

        public void Initialize()
        {
            int k = 0;

            foreach (GameEntity e in _boidsGroup)
            {
                GameObject linkedGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
                linkedGO.name = $"Linked_GO_{k}";
                linkedGO.transform.localScale = Vector3.one * .25f;
                linkedGO.transform.SetParent(parentGO.transform);

                e.ReplaceLinkedGO(linkedGO);

                k++;
            }
        }
    }
}