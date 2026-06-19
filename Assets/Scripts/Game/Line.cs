using UnityEngine;

namespace Game
{
    public enum EntityType
    {
        None,
        Enemy,
        Coin,
        Villager
    }
    public struct Line
    {
        public EntityType left, right;

        public Line(EntityType left, EntityType right)
        {
            this.left = left;
            this.right = right;
        }

        public void Destroy(Choose choose)
        {
            EntityType destroyed = EntityType.None;
            if (choose == Choose.Left)
            {
                destroyed = left;
                left = EntityType.None;
            }

            if (choose == Choose.Right)
            {
                destroyed = right;
                right = EntityType.None;
            }
            Debug.Log($"Destroyed {(int) destroyed}");
        }

        public void Destroy()
        {
            left = right = EntityType.None;
        }
    }
}