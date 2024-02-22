using System.Collections.Generic;
using Enemies;

namespace Managers
{
    public class EntityManager : Singleton<EntityManager>
    {
        public List<Enemy> aliveEnemies = new List<Enemy>();
        public List<Enemy> defeatedEnemies = new List<Enemy>();
        
        public void RegisterEnemy(Enemy enemy)
        {
            if (!aliveEnemies.Contains(enemy))
                aliveEnemies.Add(enemy);
        }
        
        public void UnregisterEnemy(Enemy enemy)
        {
            if (aliveEnemies.Contains(enemy) && !defeatedEnemies.Contains(enemy))
            {
                defeatedEnemies.Add(enemy);
                aliveEnemies.Remove(enemy);
            }
        }
    }
}