using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game
{
    public class LineManager : MonoBehaviour
    {
        const int Step = 1;
        public int maximumLines = 5;
        public float radius = 2; // Расстояние от центра до сущности на линии
        public float height = 2; // Высота линии
        public Vector3 startPoint = Vector3.zero;
        public GameObject prefab;
        private Queue<Line> _lines = new Queue<Line>();

        public void CreateLine()
        {
            if (prefab == null)
                throw new ArgumentException("prefab is null");

            Vector3 left = startPoint;
            Vector3 right = startPoint;
            left.y += height;
            right.y += height;
            left.x -= radius;
            right.x += radius;
            _lines.Enqueue(new Line(
                Instantiate(prefab, left, Quaternion.identity),
                Instantiate(prefab, right, Quaternion.identity)
            ));

            if (_lines.Count > maximumLines)
            {
                var line = _lines.Dequeue();
                Destroy(line.left);
                Destroy(line.right);
            }
                
        }

        public void StepDown()
        {
            foreach (var item in _lines)
            {
                item.left.transform.position += Vector3.down * Step;
                item.right.transform.position += Vector3.down * Step;
            }
        }

        public void DestroyFirstLine()
        {
            Line line = _lines.Dequeue();
            Destroy(line.left);
            Destroy(line.right);
        }

        public Line Peek()
            => _lines.Peek();
    }

    public struct Line
    {
        public GameObject left, right;

        public Line(GameObject left, GameObject right)
        {
            this.left = left;
            this.right = right;
        }
    }
}