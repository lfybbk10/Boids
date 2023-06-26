using System.Collections;
using System.Collections.Generic;
using D_Framework;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

namespace Tests
{
    public class BoidsMathUtilityTest
    {
        private const float TARGET_PRECISION = 0.01f;
        private const int EQUIVALENCE_ITERATION_COUNT = 1000;
        private const float MIN_TEST_RANGE_VALUE = -10_000f;
        private const float MAX_TEST_RANGE_VALUE = 10_000f;
        
        private readonly D_Rng rng;

        private float NextTestFloat => rng.RollRandomFloatInRange(MIN_TEST_RANGE_VALUE, MAX_TEST_RANGE_VALUE);
        
        public BoidsMathUtilityTest()
        {
            int seed = UnityEngine.Random.Range(0, int.MaxValue);
            rng = new D_Rng(seed);
            
            Debug.Log($"Math utility test runner instance created with seed {seed}");
        }
        
        [Test]
        [Repeat(EQUIVALENCE_ITERATION_COUNT)]
        public void GetClampedDirectionEquivalenceTest()
        {
            Vector3 vector = new(NextTestFloat, NextTestFloat, NextTestFloat);
            Vector3 velocity = new(NextTestFloat, NextTestFloat, NextTestFloat);
            float maxSpeed = NextTestFloat;
            float maxSteerForce = NextTestFloat;
            
            float3 mathematicsValue = BoidsMathUtility.GetClampedDirection(vector, velocity, 
                maxSpeed, maxSteerForce);
            Vector3 mathValue = BoidHelper.SteerTowards(vector, velocity,
                maxSpeed, maxSteerForce);
            
            Assert.IsTrue(math.abs(mathematicsValue.x - mathValue.x) < TARGET_PRECISION);
            Assert.IsTrue(math.abs(mathematicsValue.y - mathValue.y) < TARGET_PRECISION);
            Assert.IsTrue(math.abs(mathematicsValue.z - mathValue.z) < TARGET_PRECISION);
        }
        
        [Test]
        [Repeat(EQUIVALENCE_ITERATION_COUNT)]
        public void GetForwardDirectionEquivalenceTest()
        {
            Vector3 localDirectionVector = new(NextTestFloat, NextTestFloat,NextTestFloat);
            Vector3 parentForwardDirection = new Vector3(NextTestFloat,NextTestFloat,NextTestFloat).normalized;

            GameObject parentGO = new();
            parentGO.transform.forward = parentForwardDirection;

            float3 transformWorldVector = parentGO.transform.TransformVector(localDirectionVector);
            float3 mathematicsWorldVector = math.mul(quaternion.LookRotationSafe(parentForwardDirection, math.up()), 
                localDirectionVector);
            
            Assert.IsTrue(math.abs(transformWorldVector.x - mathematicsWorldVector.x) < TARGET_PRECISION);
            Assert.IsTrue(math.abs(transformWorldVector.y - mathematicsWorldVector.y) < TARGET_PRECISION);
            Assert.IsTrue(math.abs(transformWorldVector.z - mathematicsWorldVector.z) < TARGET_PRECISION);
        }

        [Test]
        [Repeat(EQUIVALENCE_ITERATION_COUNT)]
        public void GetAvoidanceDirectionsEquivalenceTest()
        {
            int avoidanceDirectionCount = 300;
            
            float3[] mathematicsValues = BoidsMathUtility.GetAvoidanceRayDirections(avoidanceDirectionCount);
            Vector3[] mathValues = BoidHelper.directions;

            for (int i = 0; i < avoidanceDirectionCount; i++)
            {
                Assert.IsTrue(math.abs(mathematicsValues[i].x - mathValues[i].x) < TARGET_PRECISION);
                Assert.IsTrue(math.abs(mathematicsValues[i].x - mathValues[i].x) < TARGET_PRECISION);
                Assert.IsTrue(math.abs(mathematicsValues[i].x - mathValues[i].x) < TARGET_PRECISION);
            }
        }
        
        [Test]
        [Repeat(EQUIVALENCE_ITERATION_COUNT)]
        public void ClampMagnitudeEquivalenceTest()
        {
            float x = NextTestFloat;
            float y = NextTestFloat;
            float z = NextTestFloat;
            float targetMagnitude = NextTestFloat;

            float3 mathematicsValue = BoidsMathUtility.ClampMagnitude(new float3(x, y, z), targetMagnitude);
            Vector3 mathValue = Vector3.ClampMagnitude(new Vector3(x, y, z), targetMagnitude);
            
            Assert.IsTrue(math.abs(mathematicsValue.x - mathValue.x) < TARGET_PRECISION);
            Assert.IsTrue(math.abs(mathematicsValue.y - mathValue.y) < TARGET_PRECISION);
            Assert.IsTrue(math.abs(mathematicsValue.z - mathValue.z) < TARGET_PRECISION);
        }
        
        [Test]
        public void GetClosestPointOnRayTest()
        {
            float3 origin = new(0f, 0f, 0f);
            float3 direction = math.normalizesafe(new float3(1f, 0f, 0f));
            float3 projectionSource = new(25f, 15f, 5f);

            float3 closestRayPoint = BoidsMathUtility.GetClosestPointOnRay(origin, direction, projectionSource);
            Assert.AreEqual(closestRayPoint, new float3(25f, 0f, 0f));
        }
    }
}