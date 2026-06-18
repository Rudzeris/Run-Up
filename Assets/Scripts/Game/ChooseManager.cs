using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class ChooseManager : MonoBehaviour
    {
        public Action<Choose> Choosed;

        private void Update()
        {
            Choose choose = Choose.None;
            if (Input.anyKeyDown && !EventSystem.current.IsPointerOverGameObject())
            {
                var axis = Input.GetAxis("Horizontal");
                if (axis > 0)
                    choose = Choose.Right;
                else if (axis < 0)
                    choose = Choose.Left;
                else
                    choose = Choose.Space;
                Choosed?.Invoke(choose);
            }
        }
    }

    public enum Choose
    {
        None,
        Right,
        Left,
        Space
    }
}