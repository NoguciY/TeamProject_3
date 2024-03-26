using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActive : MonoBehaviour
{
    [SerializeField] GameObject FirstBossObject;
    [SerializeField] GameObject NextBossObject;
    [SerializeField] GameObject FinalBossBossObject;
    public float SpawnTime = 300.0f;

    void Start()
    {
        FirstBossObject.SetActive(false);
        NextBossObject.SetActive(false);
        FinalBossBossObject.SetActive(false);
        Invoke("FirstBoss", SpawnTime);
    }

    void NextBoss()
    {
        NextBossObject.SetActive(true);
        Invoke("FinalBoss", SpawnTime);
    }

    void FinalBoss()
    {
        FinalBossBossObject.SetActive(true);
    }

    void FirstBoss()
    {
        FirstBossObject.SetActive(true);
        Invoke("NextBoss", SpawnTime);
    }
}