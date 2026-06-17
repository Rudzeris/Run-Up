using System;
using TMPro;
using UnityEngine;

namespace Game
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private bool _isEnabled;
        [SerializeField] private LineManager _lineManager;
        [SerializeField] private ChooseManager _chooseManager;
        [SerializeField, Range(1,3)] private int _stepForNewLine = 2;

        private int _currentStep = 0;
        
        public void StartLevel()
        {
            if (_lineManager == null)
                throw new ArgumentNullException("LineManager is null");
            _isEnabled = true;

            if (_chooseManager != null)
                _chooseManager.Choosed += OnChoosed;
        }

        private void OnChoosed(Choose obj)
        {
            if(obj == Choose.None)
                return;

            if (obj != Choose.Space)
            {
                Line line = _lineManager.Peek();
                switch (obj)
                {
                    case Choose.Left:
                        Destroy(line.left);
                        break;
                    case Choose.Right:
                        Destroy(line.right);
                        break;
                }
            }
            _lineManager.StepDown();

            if (_currentStep <= 0)
            {
                _lineManager.CreateLine();
                _currentStep = _stepForNewLine;
            }
            else
                _currentStep--;
            Debug.Log($"Current step: {_currentStep}");
        }
    }
}