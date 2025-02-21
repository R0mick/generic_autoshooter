using _Scripts.EnemiesScripts;
using UnityEngine;

public class SlimeXL : AbstractEnemy
{
    [SerializeField] private GameObject enemyHpBar;
    
    protected override void Start() //rebuild with constructor
    {
        enemyName = "SlimeXL";
        enemyMaxHealth = 150;
        enemyHealth = 150;
        enemySpeed = 0.5f;
        enemyDamage = 10;
        EnemyScore = 20;

        isMinionSpawner = true;
        isSpawnMinionAfterDeath = true;
        spawnMinionName = "SlimeM";
        spawnMinionAfterDeathName = "SlimeL";
        spawnMinionCount = 1;
        spawnAfterDeathCount = 2;
        spawnMinionRate = 2f;
            
        base.Start();
    }
    
    protected override void ChangeHpBar (float newCurrentHp)
    {
            
        var hpPart = (enemyHealth/enemyMaxHealth );
            
        enemyHpBar.transform.localScale = new Vector3(hpPart,1,1); ;
            
    }
}
