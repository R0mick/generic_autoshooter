namespace _Scripts.EnemiesScripts
{
    public class SlimeM : AbstractEnemy
    {

        protected override void Start() //rebuild with constructor
        {
            EnemyName = "SlimeM";
            EnemyMaxHealth = 4;
            EnemyHealth = 4;
            EnemySpeed = 1f;
            EnemyDamage = 2;
            EnemyScore = 2;
        
            isSpawnMinionAfterDeath = true;
            spawnMinionAfterDeathName = "SlimeS";
            spawnAfterDeathCount = 2;
            
            base.Start();
        }
    
    }

}
