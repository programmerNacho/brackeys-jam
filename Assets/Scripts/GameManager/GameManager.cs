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
        public int Money = 0;

        public CoreBlock spawnedPlayer = null;
        private List<CoreBlock> spawnedEnemies = new List<CoreBlock>();

        private Radar radar = null;

        public UnityEvent OnStartCollecting = new UnityEvent();
        public UnityEvent OnStartFighting = new UnityEvent();
        public UnityEvent OnWin = new UnityEvent();
        public UnityEvent OnLose = new UnityEvent();

        private void Start()
        {
            Money = initialMoney;
            radar = GetComponentInChildren<Radar>();

            if(rounds.Count > 0)
            {
                currentRound = rounds[0];
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
            int currentRoundIndex = rounds.IndexOf(currentRound);

            bool isLastRound = currentRoundIndex == rounds.Count - 1;
            if(isLastRound)
            {
                GameWin();
            }
            else
            {
                EliminatePlayer();

                Money = currentRound.moneyWon;
                currentRound = rounds[currentRoundIndex + 1];

                InitiateRound();
            }

            EliminateEnemies();
        }

        private void GameWin()
        {
            OnWin.Invoke();
            Debug.Log("Win!");
        }

        private void GameLose()
        {
            OnLose.Invoke();
            Debug.Log("Lose!");
        }

        private void CreatePlayer()
        {
            if(spawnedPlayer == null)
            {
                spawnedPlayer = Instantiate(playerCoreBlockPrefab, playerSpawnPoint.position, Quaternion.identity);
                radar.center = spawnedPlayer.transform;
            }
        }

        private void EliminatePlayer()
        {
            if(spawnedPlayer)
            {
                Destroy(spawnedPlayer);
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
                Instantiate(currentRound.enemies[i], enemySpawnPoints[i].position, Quaternion.identity);
            }

            CoreBlock[] allCoreBlocks = FindObjectsOfType<CoreBlock>();

            foreach (CoreBlock coreBlock in allCoreBlocks)
            {
                if (coreBlock.CurrentAffiliation == Affiliation.Enemy)
                {
                    spawnedEnemies.Add(coreBlock);
                }
            }
        }

        private void SubscribeToOnDestroy()
        {
            if(spawnedPlayer)
            {
                spawnedPlayer.GetComponent<BlockDamage>().OnBlockDestroyed.AddListener(OnPlayerKilled);
            }

            if(spawnedEnemies.Count > 0)
            {
                foreach (CoreBlock enemy in spawnedEnemies)
                {
                    if(enemy)
                    {
                        enemy.GetComponent<BlockDamage>().OnBlockDestroyed.AddListener(OnEnemyKilled);
                    }
                }
            }
        }

        private void OnPlayerKilled()
        {
            GameLose();
        }

        private void OnEnemyKilled()
        {
            bool noEnemiesAlive = true;

            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                if(spawnedEnemies[i] != null)
                {
                    noEnemiesAlive = false;
                    break;
                }
            }

            if(noEnemiesAlive)
            {
                EndRound();
            }
        }
    }
}
