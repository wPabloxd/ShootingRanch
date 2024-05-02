using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveChallenge : Wave
{
    public int waveCount;
    [SerializeField] Transform target;
    [SerializeField] GameObject[] enemiesType;
    [SerializeField] GameObject heal;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] enemiesAlive = new GameObject[300];
    [SerializeField] int numberOfEnemies;
    [SerializeField] int[] spawnQueue;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] EntityWeaponsChallenge entityWeaponsChallenge;
    int enemiesDead;
    public bool waveStarting;

    void Start()
    {
        numberOfEnemies = 7;
    }
    public override void StartWave()
    {
        waveCount++;
        text.text = waveCount.ToString();
        ResetParameters();
        SpawnWave();
    }
    void SpawnWave()
    {
        numberOfEnemies += 3;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int enemyTypeToSpawn;
            int randomGenerator = Random.Range(0, 100);
            if (randomGenerator <= 10)
            {
                enemyTypeToSpawn = 0;
            }
            else if (randomGenerator <= 60)
            {
                enemyTypeToSpawn = 1;
            }
            else
            {
                enemyTypeToSpawn = 2;
            }
            int spawnPointToSpawn = Random.Range(0, spawnPoints.Length);
            if (spawnQueue[spawnPointToSpawn] > 0)
            {
                StartCoroutine(SpawnQueue(spawnQueue[spawnPointToSpawn], enemyTypeToSpawn, spawnPointToSpawn, i));
            }
            else
            {
                if (!enemiesAlive[i])
                {
                    enemiesAlive[i] = null;
                }     
                enemiesAlive[i] = Instantiate(enemiesType[enemyTypeToSpawn], spawnPoints[spawnPointToSpawn].position, Quaternion.identity, gameObject.transform);
                enemiesAlive[i].GetComponent<Enemy>().target = target;
                enemiesAlive[i].GetComponent<EntityLife>().died.AddListener(UpdateAlive);
            }
            spawnQueue[spawnPointToSpawn]++;
            waveStarting = false;
        }
    }
    IEnumerator SpawnQueue(int delayTime, int enemyTypeToSpawn, int spawnPointToSpawn, int i)
    {
        yield return new WaitForSeconds(delayTime);
        if (!enemiesAlive[i])
        {
            enemiesAlive[i] = null;
        }
        enemiesAlive[i] = Instantiate(enemiesType[enemyTypeToSpawn], spawnPoints[spawnPointToSpawn].position, Quaternion.identity, gameObject.transform);
        enemiesAlive[i].GetComponent<Enemy>().target = target;
        enemiesAlive[i].GetComponent<EntityLife>().died.AddListener(UpdateAlive);
    }
    private void ResetParameters()
    {
        enemiesDead = 0;
        for (int i = 0; i < spawnQueue.Length; i++)
        {
            spawnQueue[i] = 0;
        }
    }
    public void UpdateAlive(Vector3 position)
    {
        enemiesDead++;
        if (entityWeaponsChallenge.nextEnemyDrops)
        {
            entityWeaponsChallenge.SpawnNewWeapon(position);
        }
        int spawnHealRandom = Random.Range(0, 100);
        if (spawnHealRandom < GameManager.Instance.spawnHealingProbability)
        {
            Instantiate(heal, new Vector3(position.x, 0, position.z), Quaternion.identity);
        }
    }
    public override bool HasFinished()
    {
        if (waveStarting)
        {
            return false;
        }
        return enemiesDead == numberOfEnemies;
    }
}