using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class ViewManager : MonoBehaviour
    {
        private const int StepY = 1;
        [SerializeField] private LineManager _lineManager;
        [SerializeField] private Vector3 _startPoint = new Vector3(0, -3, 0);
        [SerializeField] private float _radius = 2;
        [SerializeField] private TypePrefab[] _prefabs;

        private List<GameObject> _objects = new List<GameObject>();

        private void Start()
        {
            _lineManager.Moved += OnMoved;
            _lineManager.FirstLineDestroyed += OnFirstLineDestroyed;
            _lineManager.CreatedLine += OnCreatedLine;

            for (int i = 0; i < _lineManager.MaximumLines; i++)
                if (_lineManager.GetLine(i) is Line line)
                {
                    InstantiateAndConnect(line.left, -_radius, i);
                    InstantiateAndConnect(line.right, _radius, i);
                }
        }

        private void InstantiateAndConnect(Entity entity, float x, int i)
        {
            if (entity != null)
            {
                var obj = InstantiateObject(entity, x, i);
                _objects.Add(obj);
                entity.Destroyed += (e) =>
                {
                    entity.Destroyed = null;
                    _objects.Remove(obj);
                    GameObject.Destroy(obj);
                };
            }
        }

        private GameObject InstantiateObject(Entity entity, float x, int i)
        {
            var typePrefabs = _prefabs.Where(p => p.type == entity.Type).ToArray();
            if (typePrefabs.Length > 0)
            {
                var spawnPos = _startPoint;
                spawnPos.y += i;
                spawnPos.x = x;
                return Instantiate(typePrefabs[0].prefab, spawnPos, Quaternion.identity);
            }

            return null;
        }

        private void OnCreatedLine(Line line)
        {
            InstantiateAndConnect(line.left, -_radius, _lineManager.LastIndex);
            InstantiateAndConnect(line.right, _radius, _lineManager.LastIndex);
        }

        private void OnFirstLineDestroyed()
        {
            
        }

        private void OnMoved()
        {
            foreach (var obj in _objects)
            {
                if(obj==null) continue;
                var pos = obj.transform.position;
                pos.y -= StepY;
                obj.transform.position = pos;
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_startPoint, 0.3f);
        }
    }
}