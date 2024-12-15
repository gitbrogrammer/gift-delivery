//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;
using UnityEditor;
//#endregion


namespace CodeHelper
{
    public static class Helper
    {
        public static IEnumerator DelayAction(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        public static IEnumerator RegularDelayAction(Action action, float delay, int iteration)
        {

            for (int i = 1; i <= iteration; i++)
            {
                yield return new WaitForSeconds(delay);
                action();
            }
        }

        public static List<T> CheackArrayDuplicates<T>(List<T> array)
        {

            var duplicates = array.GroupBy(x => x)
                  .Where(g => g.Count() > 1)
                  .Select(y => y.Key)
                  .ToList();

            return duplicates;
        }

        public static bool VectorsAreEqual(Vector3 vector1, Vector3 vector2)
        {
            int x1 = (int)vector1.x;
            int y1 = (int)vector1.y;
            int z1 = (int)vector1.z;
            int x2 = (int)vector2.x;
            int y2 = (int)vector2.y;
            int z2 = (int)vector2.z;

            return (x1 == x2) && (y1 == y2) && (z1 == z2);
        }

#if UNITY_EDITOR
        public static GameObject InstantiatePrefab(GameObject prefab)
        {
            GameObject newPrefab = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            return newPrefab;
        }
#endif

        public static Vector3 FindClosestVector(List<Vector3> vectorList, Vector3 target)
        {
            if (vectorList == null || vectorList.Count == 0)
            {
                Debug.LogError("The vectorList is empty or null.");
                return Vector3.zero;
            }

            Vector3 closestVector = vectorList[0];
            float closestDistance = Vector3.Distance(target, closestVector);

            for (int i = 1; i < vectorList.Count; i++)
            {
                float distance = Vector3.Distance(target, vectorList[i]);
                if (distance < closestDistance)
                {
                    closestVector = vectorList[i];
                    closestDistance = distance;
                }
            }

            return closestVector;
        }

        public static List<Vector3> SortVectorsByDistance(List<Vector3> vectorList, Vector3 targetVector)
        {
            vectorList.Sort((a, b) => Vector3.Distance(a, targetVector).CompareTo(Vector3.Distance(b, targetVector)));
            return vectorList;
        }

        public static (int, int) GetIndexMatrix(int columns, int rows, int number)
        {
            // Проверка на валидность аргументов
            if (columns <= 0 || rows <= 0 || number < 0 || number >= columns * rows)
            {
                Debug.LogError("Неверные аргументы для GetGridPosition!");
                return (-1, -1); // Возврат значения по умолчанию в случае ошибки
            }

            // Вычисление координат
            int col = number % columns;
            int row = number / columns;
            // Debug.Log((col, row));
            return (col, row);
        }
    }
}
