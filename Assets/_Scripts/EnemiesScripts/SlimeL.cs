using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.EnemiesScripts
{
    public class SlimeL : AbstractEnemy
    {
        
        [SerializeField] private GameObject enemyHpBar;
        protected override void Start() //rebuild with constructor
        {
            EnemyName = "SlimeL";
            EnemyMaxHealth = 10;
            EnemyHealth = 10;
            EnemySpeed = 0.7f;
            EnemyDamage = 5;
            EnemyScore = 5;

            IsMinionSpawner = true;
            isSpawnMinionAfterDeath = true;
            spawnMinionName = "SlimeS";
            spawnMinionAfterDeathName = "SlimeM";
            spawnMinionCount = 1;
            spawnAfterDeathCount = 2;
            SpawnMinionRate = 3f;
            
            base.Start();
        }


        protected override void ChangeHpBar (float newCurrentHp)
        {
            
            var hpPart = (EnemyHealth/EnemyMaxHealth );
            
            enemyHpBar.transform.localScale = new Vector3(hpPart,1,1); ;
            
        }
    }
}
