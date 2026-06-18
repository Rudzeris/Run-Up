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
        public Vector3 startPoint = Vector3.zero;
        public GameObject prefab;
        private Line?[] _lines;

        private void Start()
        {
            _lines = new Line?[maximumLines];
        }

        public void CreateLine()
        {
            if (prefab == null)
                throw new ArgumentException("prefab is null");

            if (_lines[LastIndex] != null)
                return;

            Vector3 left = startPoint;
            Vector3 right = startPoint;
            left.y += LastIndex;
            right.y += LastIndex;
            left.x -= radius;
            right.x += radius;

            _lines[LastIndex] = new Line(
                Instantiate(prefab, left, Quaternion.identity),
                Instantiate(prefab, right, Quaternion.identity)
            );
            ShowLinesDebug();

        }

        private int LastIndex => maximumLines - 1;

        public void StepDown()
        {
            DestroyFirstLine();
            for (int i = 0; i < LastIndex; i++)
                _lines[i] = _lines[i + 1];
            _lines[LastIndex] = null;

            for (int i = 0; i < maximumLines; i++)
                _lines[i]?.SetPosition(i+startPoint.y);
            ShowLinesDebug();
        }

        private void ShowLinesDebug()
        {
            string text = "";
            foreach (var line in _lines) text += (line == null ? "null; " : "not null; ");
            Debug.Log(text);
        }

        public void DestroyFirstLine()
        {
            Line? line = _lines[0];
            line?.Destroy();
        }

        public Line? Peek()
            => _lines[0];

        public void OnDrawGizmos()
        {
            Gizmos.color=Color.red;
            Gizmos.DrawSphere(startPoint,0.3f);
        }
    }

    public struct Line
    {
        public GameObject left, right;

        public Line(GameObject left, GameObject right)
        {
            this.left = left;
            this.right = right;
        }

        public void SetPosition(float y)
        {
            SetPositionForGameObject(y, left);
            SetPositionForGameObject(y, right);
        }

        private static void SetPositionForGameObject(float y, GameObject gameObject)
        {
            Vector3 temp = gameObject.transform.position;
            temp.y = y;
            gameObject.transform.position = temp;
        }

        public void Destroy(Choose choose)
        {
            if (choose == Choose.Left)
            {
                GameObject.Destroy(left);
                left = null;
            }

            if (choose == Choose.Right)
            {
                GameObject.Destroy(right);
                right = null;
            }
        }

        public void Destroy()
        {
            if (left != null) Destroy(Choose.Left);
            if (right != null) Destroy(Choose.Right);
        }
    }
}