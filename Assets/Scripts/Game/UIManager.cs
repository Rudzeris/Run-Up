using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Text enemies;
        [SerializeField] private Text coins;
        [SerializeField] private Text villagers;

        public void ShowEnemies(int count) => enemies.text = count.ToString();
        public void ShowCoins(int count) => coins.text = count.ToString();
        public void ShowVillagers(int count) => villagers.text = count.ToString();
    }
}