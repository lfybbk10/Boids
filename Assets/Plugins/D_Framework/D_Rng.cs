using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace D_Framework {

    /// <summary>
    /// Thread-safe implementation of Random Number Generator which makes use of noise function as a fundamental for 
    /// generating a sequence of pseudorandom numbers. Does not have a state and, therefore, may be called from different threads and
    /// allows backtracking. See Squirrel Eiserloh's "Noise-Based RNG" talk at 2017 GDC for more information.
    /// </summary>

    public class D_Rng
    {
        private int m_seed;
        private int m_position;

        public D_Rng(int seed)
        {
            m_seed = seed;
            m_position = 0;
        }

        public int GetSeed() => m_seed;
        public int GetPosition() => m_position;

        public void ResetSeed(int seed, int position = 0)
        {
            m_seed = seed;
            m_position = position;
        }

        public void SetPosition(int position) => m_position = position;

        public int GetRandomInt()
        {
            int BIT_NOISE_1 = 0xB5297A4;
            int BIT_NOISE_2 = 0x68E31DA;
            int BIT_NOISE_3 = 0x1B56C4E;

            int mangled = m_position;
            mangled *= BIT_NOISE_1;
            mangled += m_seed;
            mangled ^= mangled >> 8;
            mangled += BIT_NOISE_2;
            mangled ^= mangled << 8;
            mangled *= BIT_NOISE_3;
            mangled ^= mangled >> 8;

            m_position++;
            return mangled;
        }

        public float RollRandomFloatInRange(float minValueInclusive, float maxValueInclusive)
        {
            return GetRandomInt().Remap(0, int.MaxValue, minValueInclusive, maxValueInclusive);
        }
        public float RollRandomFloatZeroToOne()
        {
            return GetRandomInt().Remap(0, int.MaxValue, 0f, 1f);
        }
        public float RollRandomFloatMinusOneToOne()
        {
            return GetRandomInt().Remap(0, int.MaxValue, -1f, 1f);
        }
        public int RollRandomIntInRange(int minValueInclusive, int maxValueInclusive)
        {
            return (int)GetRandomInt().Remap(0, int.MaxValue, (float)minValueInclusive, (float)maxValueInclusive);
        }
        public bool RollRandomChance(float probabilityOfReturningTrue)
        {
            return GetRandomInt() < int.MaxValue * probabilityOfReturningTrue;
        }
        public void RollRandomDirection2D(out float x, out float y)
        {
            x = RollRandomFloatZeroToOne();
            y = RollRandomFloatZeroToOne();
        }
        public Vector3 RollRandomUnitVector()
        {
            return new Vector3(
                RollRandomFloatMinusOneToOne(),
                RollRandomFloatMinusOneToOne(),
                RollRandomFloatMinusOneToOne()
                ).normalized;
        }

        // Methods not advancing the position

        public int Get1dNoiseInt(int positionX)
        {
            int BIT_NOISE_1 = 0xB5297A4;
            int BIT_NOISE_2 = 0x68E31DA;
            int BIT_NOISE_3 = 0x1B56C4E;

            int mangled = positionX;
            mangled *= BIT_NOISE_1;
            mangled += m_seed;
            mangled ^= mangled >> 8;
            mangled += BIT_NOISE_2;
            mangled ^= mangled << 8;
            mangled *= BIT_NOISE_3;
            mangled ^= mangled >> 8;

            return mangled;
        }
        public int Get2dNoiseInt(int positionX, int positionY)
        {
            int PRIME_1 = 198491317;

            return Get1dNoiseInt(positionX + (PRIME_1 * positionY));
        }
        public int Get3dNoiseInt(int positionX, int positionY, int positionZ)
        {
            int PRIME_1 = 198491317;
            int PRIME_2 = 6542989;

            return Get1dNoiseInt(positionX + (PRIME_1 * positionY) + (PRIME_2 * positionZ));
        }


        public float Get1dNoiseZeroToOne(int positionX)
        {
            return Get1dNoiseInt(positionX).Remap(0, int.MaxValue, 0f, 1f);
        }
        public float Get2dNoiseZeroToOne(int positionX, int positionY)
        {
            return Get2dNoiseInt(positionX, positionY).Remap(0, int.MaxValue, 0f, 1f);
        }
        public float Get3dNoiseZeroToOne(int positionX, int positionY, int positionZ)
        {
            return Get3dNoiseInt(positionX, positionY, positionZ).Remap(0, int.MaxValue, 0f, 1f);
        }


        public float Get1dNoiseMinusOneToOne(int positionX)
        {
            return Get1dNoiseInt(positionX).Remap(0, int.MaxValue, -1f, 1f);
        }
        public float Get2dNoiseMinusOneToOne(int positionX, int positionY)
        {
            return Get2dNoiseInt(positionX, positionY).Remap(0, int.MaxValue, -1f, 1f);
        }
        public float Get3dNoiseMinusOneToOne(int positionX, int positionY, int positionZ)
        {
            return Get3dNoiseInt(positionX, positionY, positionZ).Remap(0, int.MaxValue, -1f, 1f);
        }
    }
}