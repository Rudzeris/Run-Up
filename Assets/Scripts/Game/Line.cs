using System;
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
    public class Line
    {
        public Action Destroyed;
        public Entity left, right;

        public Line(Entity left, Entity right)
        {
            this.left = left;
            this.right = right;
        }

        public void Destroy(Choose choose)
        {
            Entity? destroyed = null;
            if (choose == Choose.Left)
            {
                destroyed = left;
                left = null;
            }

            if (choose == Choose.Right)
            {
                destroyed = right;
                right = null;
            }

            if (destroyed != null)
            {
                Debug.Log($"---Destroyed {destroyed}---");
                destroyed.Destroy();
            }
        }

        public void Destroy()
        {
            left?.Destroy();
            right?.Destroy();
            left = right = null;
            Destroyed?.Invoke();
        }

        public override string ToString() => "{" + $"{left}, {right}" + "}";
    }

    public class Entity
    {
        public Action<Entity> Destroyed;
        public EntityType Type { get; private set; }

        public Entity(EntityType type)
        {
            Type = type;
        }

        public override string ToString() => Type.ToString();

        public void Destroy()
        {
            Type = EntityType.None;
            Destroyed?.Invoke(this);
        }
    }
}