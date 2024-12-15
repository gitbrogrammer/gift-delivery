//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion


namespace RandomHelper
{
    public static class Helper
    {
        [Serializable]
        public class WeightingRandomArrayElement
        {
            [PreviewField] public GameObject prefab;
            [Range(0, 100)] public float chance;
        }


        public static float RandomFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static bool RandomBool()
        {
            return UnityEngine.Random.Range(0, 2) == 1;
        }

        public static int RandomInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static Vector2 RandomVector2(Vector2 range)
        {
            return new Vector2(UnityEngine.Random.Range(range.x, range.y), UnityEngine.Random.Range(range.x, range.y));
        }

        public static Vector2 RandomVector3(Vector3 range)
        {
            return new Vector3(UnityEngine.Random.Range(range.x, range.y), UnityEngine.Random.Range(range.x, range.y), UnityEngine.Random.Range(range.x, range.y));
        }

        public static GameObject WeightingElementArrayRandom(List<WeightingRandomArrayElement> list)
        {
            float total = 0;

            foreach (WeightingRandomArrayElement randomArrayElement in list)
            {
                total += randomArrayElement.chance;
            }

            float randomPoint = UnityEngine.Random.value * total;

            for (int i = 0; i < list.Count; i++)
            {
                if (randomPoint < list[i].chance)
                {
                    return list[i].prefab;
                }
                else
                {
                    randomPoint -= list[i].chance;
                }
            }
            return list[list.Count - 1].prefab;
        }
    }
}
