using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.EnemiesScripts;
using UnityEngine;

namespace _Scripts
{
    public class GameManager : MonoBehaviour //todo make sequences more readable and changeable
    {
        

        [SerializeField] private SettingsPopup settingsPopup;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private EnemySpawner spawner;
        [SerializeField] private PlayerBehaviour player;
        [SerializeField] private StartMenu startMenu;
        private Stopwatch _stopwatch;
        //private Coroutine cor;
        private List<Coroutine> _coroutines;
        private int _timeFromStart;
        private bool _isPlayerDead;

        private bool _isGameOver;
        private float _score;
    
        private int _enemiesSpawned;
        private int _enemiesKilled;
        
        [SerializeField] private bool _isEndlessSequenceStarted = false;

        private bool _isWeaponFirstEquipped;
        private bool _isWeaponSecondEquipped;
        private bool _isWeaponThirdEquipped;
        private bool _isWeaponForthEquipped;
        private bool _isWeaponFifthEquipped;

        private float _spawnIntervalForS;
        private int _spawnCountForS;
        private bool _ifCoroutineStartedS;

        private float _spawnIntervalForM;
        private int _spawnCountForM;
        private bool _ifCoroutineStartedM;

        private float _spawnIntervalForL;
        private int _spawnCountForL;
        private bool _ifCoroutineStartedL;
        
        private float _spawnIntervalForXL;
        private int _spawnCountForXL;
        private bool _ifCoroutineStartedXL;
        
        private bool _ifFinalWaveTriggered;
        private bool _isWaveOneBossKilled;


        public static Action<float> OnGameOverLoose;
        public static Action OnGameOverWin;
    
        private void Awake()
        {
            _stopwatch = GetComponent<Stopwatch>();
        }

        void Start()
        {
            PlayerBehaviour.OnPlayerDeath += GameOverLoose;
            AbstractEnemy.OnGiveScore += AddScore;
            spawner.OnEnemySpawn += EnemySpawned;
            AbstractEnemy.OnDeathAbstractEnemy += EnemyKilled;

            _coroutines = new List<Coroutine>();


            if (startMenu.gameObject.activeSelf)
            {
                StartMenu.OnStartGame += StartGame;
                audioManager.PlayMusic(AudioManager.MusicTracks.MenuMusic);
            }
            else
            {
                EndlessGameSequence();
            }


        }

        void Update()
        {
            //Debug.Log("spawned - "+_enemiesSpawned);
            //Debug.Log("killed - "+_enemiesKilled);
            if (!_isGameOver)
            {
                if (!_isPlayerDead)
                {
                    OpenPopup();
                }

                if (_isEndlessSequenceStarted)
                {
                    SpawningTimeLine();
                    ChangeWeaponTimeLine();
                }
            }
        }

        void OnDisable()
        {
            StopSpawning();
            PlayerBehaviour.OnPlayerDeath -= GameOverLoose;
            AbstractEnemy.OnGiveScore -= AddScore;
            spawner.OnEnemySpawn -= EnemySpawned;
            AbstractEnemy.OnDeathAbstractEnemy -= EnemyKilled;
        }

        private void SpawningTimeLine() //todo as struct
        {
            if (_timeFromStart > 0)
            {
                if (!_ifCoroutineStartedS)
                {
                    Debug.Log("Wave 1");
                    _spawnIntervalForS = 2f;
                    _spawnCountForS = 2;
                    _coroutines.Add(StartCoroutine(SpawnSlimesS()));
                    _ifCoroutineStartedS = true;
                }
            }

            if (_timeFromStart > 30 && Time.timeScale < 60)
            {
                _spawnIntervalForS = 3f;
                _spawnCountForS = 2;

                if (!_ifCoroutineStartedM)
                {
                    Debug.Log("Wave 2");
                    _spawnIntervalForM = 5f;
                    _spawnCountForM = 1;
                    _coroutines.Add(StartCoroutine(SpawnSlimesM()));
                    _ifCoroutineStartedM = true;
                }
            }

            if (_timeFromStart > 60 && Time.timeScale < 152)
            {
                _spawnIntervalForS = 4f;
                _spawnCountForS = 2;

                _spawnIntervalForM = 8f;
                _spawnCountForM = 2;

                if (!_ifCoroutineStartedL)
                {
                    Debug.Log("Wave 3");
                    _spawnIntervalForL = 12f;
                    _spawnCountForL = 1;
                    _coroutines.Add(StartCoroutine(SpawnSlimesL()));
                    _ifCoroutineStartedL = true;
                }
            }

            if (_timeFromStart > 152)
            {
                _spawnIntervalForS = 5f;
                _spawnCountForS = 2;

                _spawnIntervalForM = 10f;
                _spawnCountForM = 2;

                _spawnIntervalForL = 15f;
                _spawnCountForL = 1;


                if (!_ifCoroutineStartedXL)
                {
                    Debug.Log("Boss Wave");
                    audioManager.PlayMusic(AudioManager.MusicTracks.BossMusic);
                    _spawnIntervalForXL = 500f;
                    _spawnCountForXL = 1;
                    _coroutines.Add(StartCoroutine(SpawnSlimesXL()));
                    _ifCoroutineStartedXL = true;
                }
            }

            if (_isWaveOneBossKilled)
            {
                //Debug.Log("triggered final");
                if (!_ifFinalWaveTriggered)
                {
                    StopSpawning();

                    StartCoroutine(CheckWinCondition());
                    _ifFinalWaveTriggered = true;
                }
            }
        }

        IEnumerator CheckWinCondition() //handle enemy spawn/kill gap
        {
            while (!_isGameOver)
            {
                Debug.Log("checking if mobs killed");
                yield return new WaitForSeconds(2f);
                if (_enemiesSpawned - _enemiesKilled == 0)
                {
                    Debug.Log("game win");
                    GameOverWin();
                }
            }
        }

        private void ChangeWeaponTimeLine() //todo as struct
        {
            if (_timeFromStart > 0)
            {
                if (!_isWeaponFirstEquipped)
                {
                    player.ChangeGuns("Revolver");
                    Debug.Log("Revolver equipped");
                    _isWeaponFirstEquipped = true;
                }
            }

            if (_timeFromStart > 40)
            {
                if (!_isWeaponSecondEquipped)
                {
                    player.ChangeGuns("Pistol");
                    Debug.Log("Pistol equipped");
                    _isWeaponSecondEquipped = true;
                }
            }

            if (_timeFromStart > 70)
            {
                if (!_isWeaponThirdEquipped)
                {
                    player.ChangeGuns("ShotgunSemiAuto");
                    Debug.Log("ShotgunSemiAuto equipped");
                    _isWeaponThirdEquipped = true;
                }
            }
            
            if (_timeFromStart > 120)
            {
                if (!_isWeaponForthEquipped)
                {
                    player.ChangeGuns("Uzi");
                    Debug.Log("Uzi equipped");
                    _isWeaponForthEquipped = true;
                }
            }
            
            if (_timeFromStart > 170)
            {
                if (!_isWeaponFifthEquipped)
                {
                    player.ChangeGuns("Machinegun");
                    Debug.Log("Machinegun equipped");
                    _isWeaponFifthEquipped = true;
                }
            }


        }
    

        void OpenPopup()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!settingsPopup.gameObject.activeSelf)
                {
                    settingsPopup.Open();
                    Time.timeScale = 0;
                    audioManager.PauseSounds();
                }
                else
                {
                    settingsPopup.Close();
                    Time.timeScale = 1;
                    audioManager.ResumeSounds();
                }
            }
        }

        private void AddScore(float score)
        {
            _score+=score;
        }

        IEnumerator SpawnSlimesS()
        {
            while (true)
            {
                StartCoroutine(spawner.SpawnEnemy("SlimeS", _spawnCountForS));
                yield return new WaitForSeconds(_spawnIntervalForS);
                //Debug.LogFormat("enemy name: {0} count {1} interval {2}","SlimeS",_spawnCountForS,_spawnIntervalForS);
            }
        }

        IEnumerator SpawnSlimesM()
        {
            while (true)
            {
                StartCoroutine(spawner.SpawnEnemy("SlimeM", _spawnCountForM));
                yield return new WaitForSeconds(_spawnIntervalForM);
            }
        }

        IEnumerator SpawnSlimesL()
        {
            while (true)
            {
                StartCoroutine(spawner.SpawnEnemy("SlimeL", _spawnCountForL));
                yield return new WaitForSeconds(_spawnIntervalForL);

            }
        }
        IEnumerator SpawnSlimesXL()
        {
            while (true)
            {
                StartCoroutine(spawner.SpawnEnemy("SlimeXL", _spawnCountForXL));
                yield return new WaitForSeconds(_spawnIntervalForXL);

            }
        }

        private void StopSpawning()
        {
            foreach (var spawningCoroutine in _coroutines)
            {
                StopCoroutine(spawningCoroutine);
            }
        }

        private void StartGame(string gameModName)
        {
            switch (gameModName)
            {
                case "Arena":
                {
                    EndlessGameSequence();
                    break;
                }
                case "Campaign":
                {
                    CampaignGameSequence();
                    break;
                }
            }
        }

        private void EnemySpawned()
        {
            _enemiesSpawned++;
        }
        
        private void EnemyKilled(GameObject enemy)
        {
            _enemiesKilled++;
            if (enemy.name.Contains("SlimeXL"))
            {
                _isWaveOneBossKilled = true;
            }
        }


        private void ChangeTimeFromStart(int time)
        {
            _timeFromStart = time;
        }

        private void EndlessGameSequence()
        {
            Debug.Log("Endless Arena Started");
            StartMenu.OnStartGame -= StartGame;
            startMenu.CloseStartMenu();
            audioManager.PlayMusic(AudioManager.MusicTracks.FirstLevelMusic);
            Stopwatch.OnTimerChanged += ChangeTimeFromStart;
            _stopwatch.StartTimer();
            _isEndlessSequenceStarted = true;

        }
        

        private void CampaignGameSequence()
        {
            //implement company
        }

        private void GameOverLoose()
        {
            _stopwatch.StopTimer();
            _isGameOver = true;
            audioManager.PlayMusic(AudioManager.MusicTracks.GameOverLoseMusic);
            player.ChangeGuns("Empty");
            StopAllCoroutines();
            StopSpawning();
            _isPlayerDead = true;
            OnGameOverLoose?.Invoke(_score);
        }
        
        private void GameOverWin()
        {
            _stopwatch.StopTimer();
            _isGameOver = true;
            audioManager.PlayMusic(AudioManager.MusicTracks.GameOverWinMusic);
            StopAllCoroutines();
            StopSpawning();
            OnGameOverWin?.Invoke();
        }
    }
}

