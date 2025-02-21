namespace _Scripts.EnemiesScripts
{
    public class SlimeM : AbstractEnemy
    {

        protected override void Start() //rebuild with constructor
        {
            enemyName = "SlimeM";
            enemyMaxHealth = 4;
            enemyHealth = 4;
            enemySpeed = 1f;
            enemyDamage = 2;
            EnemyScore = 2;
        
            isSpawnMinionAfterDeath = true;
            spawnMinionAfterDeathName = "SlimeS";
            spawnAfterDeathCount = 2;
            
            base.Start();
        }
    
    }

}
