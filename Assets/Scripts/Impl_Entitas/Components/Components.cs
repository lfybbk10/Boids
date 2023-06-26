using UnityEngine;
using Entitas;
using Unity.Mathematics;

[Game] public class FollowingTargetComponent : IComponent { public float3 value; }
[Game] public class PositionComponent : IComponent { public float3 value; }
[Game] public class RotationComponent : IComponent { public float3 value; }
[Game] public class VelocityComponent : IComponent { public float3 value; }
[Game] public class AccelerationComponent : IComponent { public float3 value; }
[Game] public class FlockmateNumberComponent: IComponent { public int value; }
[Game] public class FlockCenterComponent : IComponent { public float3 value; }
[Game] public class AverageFlockDirectionComponent : IComponent { public float3 value; }
[Game] public class AverageAvoidanceComponent : IComponent { public float3 value; }
[Game] public class IsOnCollisionTrajectoryComponent : IComponent { public bool value; }
[Game] public class TargetOffsetComponent : IComponent { public float3 value; }
[Game] public class AlignmentComponent : IComponent { public float3 value; }
[Game] public class CohesionComponent : IComponent { public float3 value; }
[Game] public class SeparationComponent : IComponent { public float3 value; }
[Game] public class AvoidanceComponent : IComponent { public float3 value; }
[Game] public class LinkedGOComponent : IComponent { public GameObject linkedGO; }