namespace _Scripts.EnemiesScripts
{
    public class SlimeS : AbstractEnemy
    {
        protected override void Start() //rebuild with constructor
        {
            enemyName = "SlimeS";
            enemyMaxHealth = 1;
            enemyHealth = 1;
            enemySpeed = 1.3f;
            enemyDamage = 1;
            EnemyScore = 1;
            
            base.Start();
        }

    }

}