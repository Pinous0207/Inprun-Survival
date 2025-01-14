using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Monster_Spawner : MonoBehaviour
{
    public Monster monster;
    public int minMonsters;
    public int maxMonster;
    public float spawnRadius;
    public float detectionRadius;

    public List<Monster> spawnedMonsters = new List<Monster>();
    private Transform player;

    private void Start()
    {
        player = P_Movement.instance.transform;
        SpawnMonsters();
    }

    private void Update()
    {
        CheckPlayerDistance();
        if (spawnedMonsters.Count == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void SpawnMonsters()
    {
        int monsterCount = Random.Range(minMonsters, maxMonster + 1);
        for(int i = 0; i < monsterCount; i++)
        {
            Vector3 spawnPosition = GetRandomPosition();
            var newMonster = Instantiate(monster, spawnPosition, 
                Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
            newMonster.Init(this);
            spawnedMonsters.Add(newMonster);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomCircle = Random.insideUnitSphere * spawnRadius;
        return new Vector3(
            transform.position.x + randomCircle.x,
            transform.position.y, 
            transform.position.z + randomCircle.z);
    }

    private void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if(distance < detectionRadius)
        {
            TriggerMonsterCheck(true);
        }
        else if(distance >= detectionRadius)
        {
            TriggerMonsterCheck(false);
        }
    }

    private void TriggerMonsterCheck(bool GetPlayer)
    {
        for (int i = 0; i < spawnedMonsters.Count; i++)
        {
            if (GetPlayer)
                spawnedMonsters[i].GetPlayer(player);
            else if (!GetPlayer)
                spawnedMonsters[i].RemovePlayer();
        }
    }
}
