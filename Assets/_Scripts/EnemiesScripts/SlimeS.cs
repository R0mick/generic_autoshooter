namespace _Scripts.EnemiesScripts
{
    public class SlimeS : AbstractEnemy
    {
        protected override void Start() //rebuild with constructor
        {
            EnemyName = "SlimeS";
            EnemyMaxHealth = 1;
            EnemyHealth = 1;
            EnemySpeed = 1.3f;
            EnemyDamage = 1;
            EnemyScore = 1;
            
            base.Start();
        }

    }

}