using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //���C����ʂ̌o�ߎ���(�b)
    private float deltaTimeInMain;

    //�Q�[���N���A��������邽��
    //[SerializeField]
    //private EnemyBossSpawn enemyBossSpawn;

    //�Q�[���N���A���ǂ���
    //private bool isGameClear = false;

    //�Q�b�^�[
    public float GetDeltaTimeInMain => deltaTimeInMain;

    private static GameManager instance;

    private void Update()
    {
        //���Ԃ��v��
        deltaTimeInMain += Time.deltaTime;

        //isGameClear = enemyBossSpawn.CheckClearFlag();
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private static void SetupInstance()
    {
        instance = FindObjectOfType<GameManager>();

        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "GameManager";
            instance = gameObj.AddComponent<GameManager>();
            DontDestroyOnLoad(gameObj);
        }
    }
}
