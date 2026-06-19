using System;
using TMPro;
using UnityEngine;

namespace Game
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private UIManager _ui;
        [SerializeField] private bool _isEnabled;
        [SerializeField] private LineManager _lineManager;
        [SerializeField] private ChooseManager _chooseManager;
        [SerializeField, Range(1, 3)] private int _stepForNewLine = 2;

        private int _currentStep = 0;

        private int _countEnemies = 0;
        private int _countCoins = 0;
        private int _countVillagers = 0;

        public void Start()
        {
            if (_chooseManager != null)
                _chooseManager.Choosed += OnChoosed;
        }

        public void StartLevel()
        {
            if (_lineManager == null)
                throw new ArgumentNullException("LineManager is null");
            _isEnabled = true;

            _lineManager.CreateLine();
            _currentStep = _stepForNewLine;
        }

        private void OnChoosed(Choose choose)
        {
            if (choose == Choose.None || !_isEnabled)
                return;

            if (choose != Choose.Space)
            {
                // TODO: Вынести отсюда
                var line = _lineManager.Peek();
                var entity = choose == Choose.Left ? line?.left : line?.right;
                if (entity != null)
                {
                    switch (entity.Type)
                    {
                        case EntityType.Coin: _countCoins++;
                            break;
                        case EntityType.Enemy: _countEnemies++;
                            break;
                        case EntityType.Villager: _countVillagers++;
                            break;
                    }
                    _ui.ShowCoins(_countCoins);
                    _ui.ShowEnemies(_countEnemies);
                    _ui.ShowVillagers(_countVillagers);
                }
                line?.Destroy(choose);
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