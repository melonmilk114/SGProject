using System.Collections.Generic;

namespace Melon
{
    public interface IObjectPoolConfigurator
    {
        public List<GameElement> GetPoolPrefabs();
    }
}