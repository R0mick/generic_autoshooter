using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.EnemiesScripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        
        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private GameObject spawnPointPrefab;

        private float _spawnAreaWidth;
        private float _spawnAreaHeight;
        
        public Action OnEnemySpawn;

        //public int spawnedMobsCountInSpawner;
        private void Awake()
        {
            _spawnAreaWidth = GetComponent<SpriteRenderer>().bounds.size.x;
            _spawnAreaHeight = GetComponent<SpriteRenderer>().bounds.size.y;

        }
        

        public void SpawnButton(string enemyName)
        {
            StartCoroutine(SpawnEnemy(enemyName,1) );
        }
        
        //sequence for random spawning
        public IEnumerator SpawnEnemy(string enemyName, int enemyCount)
        {
            GameObject enemyPrefab = enemyPrefabs.Find((x => x.name == enemyName));
            {
                if (enemyPrefab != null)
                {
                    {
                        for (int i = 0; i < enemyCount; i++)
                        {
                            Vector3 randomPosition = GetRandomPositionInsideSquare();

                            var spawnPoint = Instantiate(spawnPointPrefab, randomPosition, Quaternion.identity);
                            yield return new WaitForSeconds(1f);
                            Destroy(spawnPoint.gameObject);

                            var spawnedEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                            OnEnemySpawn?.Invoke();
                            
                            spawnedEnemy.GetComponent<AbstractEnemy>().OnDeathCurrentEnemy += OnEnemyDeath;
                            spawnedEnemy.GetComponent<AbstractEnemy>().OnMinionSpawn += SpawnMinion;

                        }
                    }
                }
                else
                {
                    Debug.LogFormat("No enemy with name {0} was found!", enemyName);
                }
            }
        }
        
        //sequence for minion spawning
        public IEnumerator SpawnEnemy(string enemyName, int enemyCount, Vector3 position )
        {
           
            //spawn enemy from prefabs list via name
            GameObject enemyPrefab = enemyPrefabs.Find((x => x.name == enemyName));
            {
                if (enemyPrefab != null)
                {
                        for(int i=0; i<enemyCount; i++)
                        {
                            //Debug.Log("Position x before sizing = "+randomPosition.x);
                            if (i >= 1)
                            {
                                position.x += enemyPrefab.GetComponent<BoxCollider2D>().size.x;
                                //Debug.Log("Position x after sizing = "+randomPosition.x);
                            }

                            yield return true;
                            var spawnedEnemy =Instantiate(enemyPrefab, position, Quaternion.identity);
                            OnEnemySpawn?.Invoke();
                            
                            spawnedEnemy.GetComponent<AbstractEnemy>().OnDeathCurrentEnemy += OnEnemyDeath;
                            spawnedEnemy.GetComponent<AbstractEnemy>().OnMinionSpawn += SpawnMinion;
                        }
                }
                else
                {
                    Debug.LogFormat("No enemy with name {0} was found!", enemyName);
                }
            }
        }

        private void SpawnMinion(GameObject spawnedEnemy)
        {
            AbstractEnemy enemyScript = spawnedEnemy.GetComponent<AbstractEnemy>();
            
            StartCoroutine(SpawnEnemy(enemyScript.spawnMinionName, enemyScript.spawnMinionCount, spawnedEnemy.transform.position));
        }
        
        private void OnEnemyDeath(GameObject spawnedEnemy)
        {
            //Debug.Log(spawnedEnemy.GetInstanceID()+" is dead!");
            spawnedEnemy.GetComponent<AbstractEnemy>().OnDeathCurrentEnemy -= OnEnemyDeath;
            spawnedEnemy.GetComponent<AbstractEnemy>().OnDeathCurrentEnemy -= SpawnMinion;

            AbstractEnemy enemyScript = spawnedEnemy.GetComponent<AbstractEnemy>();
            if (enemyScript.isSpawnMinionAfterDeath)
            {
                //Debug.Log(spawnedEnemy+" is spawner");
                StartCoroutine(SpawnEnemy(enemyScript.spawnMinionAfterDeathName, enemyScript.spawnAfterDeathCount, spawnedEnemy.transform.position));
            }
        }
    
        //area of spawning
        private Vector3 GetRandomPositionInsideSquare()
        {
            float x = Random.Range(-_spawnAreaWidth / 2f, _spawnAreaWidth / 2f);
            float y = Random.Range(-_spawnAreaHeight / 2f, _spawnAreaHeight / 2f);
            return new Vector3(x, y, 0);
        }
        
    }
}
