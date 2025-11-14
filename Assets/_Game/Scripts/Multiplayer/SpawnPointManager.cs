using System.Linq;
using _Game.Scripts.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Multiplayer
{
    public class SpawnPointManager : SerializedMonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoint;

        private Transform _prevPoint;

        public Transform GetRandomSpawnPoint()
        {
            Transform point = _spawnPoint.GetUniqueRandomElements(1).First();

            if (_prevPoint == point)
               return GetRandomSpawnPoint();

            return point;
        }
    }
}