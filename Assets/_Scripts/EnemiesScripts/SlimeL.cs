using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.EnemiesScripts
{
    public class SlimeL : AbstractEnemy
    {
        
        [SerializeField] private GameObject enemyHpBar;
        protected override void Start() //rebuild with constructor
        {
            enemyName = "SlimeL";
            enemyMaxHealth = 10;
            enemyHealth = 10;
            enemySpeed = 0.7f;
            enemyDamage = 5;
            EnemyScore = 5;

            isMinionSpawner = true;
            isSpawnMinionAfterDeath = true;
            spawnMinionName = "SlimeS";
            spawnMinionAfterDeathName = "SlimeM";
            spawnMinionCount = 1;
            spawnAfterDeathCount = 2;
            spawnMinionRate = 3f;
            
            base.Start();
        }


        protected override void ChangeHpBar (float newCurrentHp)
        {
            
            var hpPart = (enemyHealth/enemyMaxHealth );
            
            enemyHpBar.transform.localScale = new Vector3(hpPart,1,1); ;
            
        }
    }
}
