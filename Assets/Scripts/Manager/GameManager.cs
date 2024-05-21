using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SceneType
{
    Title,      //�^�C�g��
    MainGame,   //���C���Q�[��
    GameOver,   //�Q�[���I�[�o�[
    Result,     //���U���g
}

public class GameManager : MonoBehaviour
{
    //�V���O���g���C���X�^���X
    private static GameManager instance;

    //���݂̃V�[��
    private SceneType currentSceneType;

    //�O��̃V�[��
    private SceneType preSceneType;

    //���C����ʂ̌o�ߎ���(�b)
    private float deltaTimeInMain = 0;

    //�ŏI���x��
    public int lastPlayerLevel = 0;

    //�|�����G�̐�
    public int deadEnemyMun = 0;

    //�Q�b�^�[
    public float GetDeltaTimeInMain => deltaTimeInMain;



    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);

        //�J�n���̃V�[��
        currentSceneType = SceneType.MainGame;
        preSceneType = currentSceneType;
    }

    private void Update()
    {
        //���C���Q�[����ʂ̏ꍇ
        if (currentSceneType == SceneType.MainGame)
            //���Ԃ��v��
            deltaTimeInMain += Time.deltaTime;

        
    }

    //GameManager�C���X�^���X�ɃA�N�Z�X����
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                //�C���X�^���X���Z�b�g
                SetupInstance();
            return instance;
        }
    }

    //�C���X�^���X���Z�b�g����
    private static void SetupInstance()
    {
        //�V�[�����ɑ��݂���GameManager������
        instance = FindObjectOfType<GameManager>();

        //�V�[�����ɂȂ��ꍇ�A�C���X�^���X���쐬����
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "GameManager";
            instance = gameObj.AddComponent<GameManager>();
            DontDestroyOnLoad(gameObj);
        }
    }

    public void ResetResult()
    {
        deltaTimeInMain = 0;
        lastPlayerLevel = 0;
        deadEnemyMun = 0;
    }

    /// <summary>
    /// ���݂̃V�[����ύX����
    /// </summary>
    /// <param name="nextSceneType">���̃V�[��</param>
    public void ChangeSceneType(SceneType nextSceneType)
    {
        preSceneType = currentSceneType;
        currentSceneType = nextSceneType;
        Debug.Log($"���݂̃V�[���F{currentSceneType}");
    }

    /// <summary>
    /// ���݂̃V�[����ύX����
    /// </summary>
    /// <param name="loadSceneName">���̃V�[����</param>
    public void ChangeSceneType(string nextSceneName)
    {
        preSceneType = currentSceneType;

        //�V�[��������SceneType�ɕϊ�
        if (nextSceneName == "TitleScene")
        { 
            currentSceneType = SceneType.Title; 
            ResetResult(); 
        }
        else if (nextSceneName == "MainScene")
            currentSceneType = SceneType.MainGame;
        else if (nextSceneName == "ResultScene")
            currentSceneType = SceneType.Result;
        else
            Debug.LogWarning($"{nextSceneName}�����݂̃V�[���ɕύX�ł��܂���");

        Debug.Log($"���݂̃V�[���F{currentSceneType}");
    }
}
