using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public int initialMoney = 500;
        public CoreBlock playerCoreBlockPrefab = null;
        public Transform playerSpawnPoint = null;
        public List<Transform> enemySpawnPoints = new List<Transform>();
        public List<Round> rounds = new List<Round>();

        private Round currentRound = null;
        private int currentRoundIndex = 0;
        public int Money = 0;

        public CoreBlock spawnedPlayer = null;

        [SerializeField]
        private List<CoreBlock> spawnedEnemies = new List<CoreBlock>();

        private Radar radar = null;

        public UnityEvent OnStartCollecting = new UnityEvent();
        public UnityEvent OnStartFighting = new UnityEvent();
        public UnityEvent OnWin = new UnityEvent();
        public UnityEvent OnLose = new UnityEvent();

        private bool alreadyWin = false;
        private bool alreadyLost = false;

        private void Start()
        {
            Money = initialMoney;
            radar = GetComponentInChildren<Radar>();

            if(rounds.Count > 0)
            {
                currentRound = rounds[0];
                currentRoundIndex = 0;
            }

            Invoke("InitiateRound", 0.1f);
        }

        private void InitiateRound()
        {
            CreatePlayer();
            CreateEnemies();
            SubscribeToOnDestroy();
            OnStartCollecting.Invoke();
        }

        public void StartFighting()
        {
            OnStartFighting.Invoke();
        }

        private void EndRound()
        {
            bool isLastRound = currentRoundIndex == rounds.Count - 1;
            if(isLastRound)
            {
                GameWin();
            }
            else
            {
                EliminatePlayer();

                Block[] blocks = FindObjectsOfType<Block>();

                foreach (var b in blocks)
                {
                    Destroy(b.gameObject);
                }

                Money += currentRound.moneyWon;
                currentRound = rounds[++currentRoundIndex];

                InitiateRound();
            }
        }

        private void GameWin()
        {
            if(!alreadyWin)
            {
                OnWin.Invoke();
                alreadyWin = true;
                FindObjectOfType<Menus>().SetCurrentMenu(Menus.Menu.win);
            }
        }

        private void GameLose()
        {
            if(!alreadyLost)
            {
                OnLose.Invoke();
                alreadyLost = true;
                FindObjectOfType<Menus>().SetCurrentMenu(Menus.Menu.fail);
            }
        }

        private void CreatePlayer()
        {
            if(!spawnedPlayer)
            {
                spawnedPlayer = Instantiate(playerCoreBlockPrefab, playerSpawnPoint.position, Quaternion.identity);
                radar.center = spawnedPlayer.transform;
            }
        }

        private void EliminatePlayer()
        {
            if(spawnedPlayer)
            {
                Destroy(spawnedPlayer.gameObject);
                spawnedPlayer = null;
            }
        }

        private void EliminateEnemies()
        {
            for (int i = spawnedEnemies.Count; i >= 0; i--)
            {
                Destroy(spawnedEnemies[i].gameObject);
                spawnedEnemies.RemoveAt(i);
            }
        }

        private void CreateEnemies()
        {
            int roundNumberOfEnemies = currentRound.enemies.Count;

            for (int i = 0; i < roundNumberOfEnemies; i++)
            {
                CoreBlock newEnemy = Instantiate(currentRound.enemies[i], enemySpawnPoints[i].position, Quaternion.identity).GetComponentInChildren<CoreBlock>();
                spawnedEnemies.Add(newEnemy);
            }
        }

        private void SubscribeToOnDestroy()
        {
            if(spawnedPlayer)
            {
                spawnedPlayer.Damage.OnBlockDestroyed.AddListener(OnPlayerKilled);
            }

            if(spawnedEnemies.Count > 0)
            {
                foreach (CoreBlock enemy in spawnedEnemies)
                {
                    if(enemy)
                    {
                        enemy.Damage.OnBlockDestroyed.AddListener(OnEnemyKilled);
                    }
                }
            }
        }

        private void OnPlayerKilled(Block blockDestroyed)
        {
            GameLose();
        }

        private void OnEnemyKilled(Block blockDestroyed)
        {
            CoreBlock coreBlock = blockDestroyed as CoreBlock;
            if (!coreBlock) return;

            blockDestroyed.Damage.OnBlockDestroyed.RemoveListener(OnEnemyKilled);

            spawnedEnemies.Remove(coreBlock);

            if (spawnedEnemies.Count <= 0)
            {
                Invoke("EndRound", 4f);
            }
        }
    }
}
