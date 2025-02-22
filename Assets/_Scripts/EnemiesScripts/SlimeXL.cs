using _Scripts.EnemiesScripts;
using UnityEngine;

public class SlimeXL : AbstractEnemy
{
    [SerializeField] private GameObject enemyHpBar;
    
    protected override void Start() //rebuild with constructor
    {
        EnemyName = "SlimeXL";
        EnemyMaxHealth = 150;
        EnemyHealth = 150;
        EnemySpeed = 0.5f;
        EnemyDamage = 10;
        EnemyScore = 20;

        IsMinionSpawner = true;
        isSpawnMinionAfterDeath = true;
        spawnMinionName = "SlimeM";
        spawnMinionAfterDeathName = "SlimeL";
        spawnMinionCount = 1;
        spawnAfterDeathCount = 2;
        SpawnMinionRate = 2f;
            
        base.Start();
    }
    
    protected override void ChangeHpBar (float newCurrentHp)
    {
            
        var hpPart = (EnemyHealth/EnemyMaxHealth );
            
        enemyHpBar.transform.localScale = new Vector3(hpPart,1,1); ;
            
    }
}
