using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    public class LineManager : MonoBehaviour
    {
        [SerializeField] private int _maximumLines = 10;
        [SerializeField] private float _radius = 2; // Расстояние от центра до сущности на линии
        [SerializeField] private Vector3 _startPoint = Vector3.zero;

        public Action FirstLineDestroyed;
        public Action Moved;
        public Action<Line> CreatedLine;
        const int Step = 1;


        public int MaximumLines => _maximumLines;
        public float Radius => _radius;
        private Vector3 StartPoint => _startPoint;
        private int LastIndex => MaximumLines - 1;


        private Line? firstLine;
        private Line?[] _lines;

        private void Start()
        {
            _lines = new Line?[MaximumLines];
        }

        public void CreateLine()
        {
            if (_lines[LastIndex] != null)
                return;

            Vector3 left = _startPoint;
            Vector3 right = _startPoint;
            left.y += LastIndex;
            right.y += LastIndex;
            left.x -= _radius;
            right.x += _radius;
            
            var newLine = new Line(
                (EntityType) (int)UnityEngine.Random.Range(1,3),
                (EntityType) (int)UnityEngine.Random.Range(1,3)
            );
            _lines[LastIndex] = newLine;

            CreatedLine?.Invoke(newLine);
            ShowLinesDebug();
        }

        public void StepDown()
        {
            firstLine?.Destroy();
            firstLine = _lines[0];
            for (int i = 0; i < LastIndex; i++)
                _lines[i] = _lines[i + 1];
            _lines[LastIndex] = null;
            Moved?.Invoke();
            ShowLinesDebug();
        }

        public void DestroyFirstLine()
        {
            Line? line = _lines[0];
            line?.Destroy();
            FirstLineDestroyed?.Invoke();
        }

        public Line? Peek()
            => _lines[0];

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_startPoint, 0.3f);
        }

        private void ShowLinesDebug()
        {
            string text = "";
            if(firstLine!=null)
                text+=$"[{(int)firstLine?.left},{(int)firstLine?.right}]; ";
            foreach (var line in _lines) text += (line == null ? "null" : 
                    $"{(int)line?.left}|{(int)line?.right}")+"; ";
            Debug.Log(text);
        }
    }
}