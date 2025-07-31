using System.Collections.Generic;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class ObjectPoolConfigurator : GameElement, IObjectPoolConfigurator
    {
        [SerializeField] private List<GameElement> poolPrefabs = new List<GameElement>();

        public List<GameElement> GetPoolPrefabs() => poolPrefabs;
    }
}