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

        public Action FirstLineDestroyed;
        public Action Moved;
        public Action<Line> CreatedLine;

        public int MaximumLines => _maximumLines;
        public int LastIndex => MaximumLines - 1;

        private Line? firstLine;
        private Line?[] _lines;

        public Line? GetLine(int index)
        {
            if (_lines == null) return null;
            if (index < 0 || index >= MaximumLines) throw new IndexOutOfRangeException("Вышли за границу lines");
            return _lines[index];
        }

        private void Start()
        {
            _lines = new Line?[MaximumLines];
        }

        public void CreateLine()
        {
            if (_lines[LastIndex] != null)
                return;

            var newLine = new Line(
                GetRandomEntity(),
                GetRandomEntity()
            );
            _lines[LastIndex] = newLine;

            CreatedLine?.Invoke(newLine);
            ShowLinesDebug();
        }

        private static Entity GetRandomEntity()
        {
            return new Entity(GetRandomEntityType());
        }

        private static EntityType GetRandomEntityType()
        {
            return (EntityType) (int) UnityEngine.Random.Range(0, 3);
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

        private void ShowLinesDebug()
        {
            string text = "";
            text += $"[{firstLine?.ToString() ?? "null"}]; ";
            foreach (var line in _lines) text += (line == null ? "null" : $"{line}") + "; ";
            Debug.Log(text);
        }
    }
}