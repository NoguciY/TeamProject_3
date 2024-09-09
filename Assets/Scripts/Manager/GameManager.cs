using System;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType
{
    Title,      //�^�C�g��
    MainGame,   //���C���Q�[��
    Option,     //�I�v�V����
    GameOver,   //�Q�[���I�[�o�[
    Result,     //���U���g
}

public class GameManager : MonoBehaviour
{
    //�V���O���g���C���X�^���X
    private static GameManager instance;

    //���݂̃V�[��
    [SerializeField]
    private SceneType currentSceneType;

    public SceneType CurrentSceneType
    {
        get { return currentSceneType; }
        set
        {
            currentSceneType = value;
            Debug.Log($"�V�[���ύX{currentSceneType}");
        }
    }

    //�O��̃V�[��
    private SceneType preSceneType;

    public SceneType PreSceneType
    {
        get { return preSceneType; }
        set { preSceneType = value; }
    }

    //���C����ʂ̌o�ߎ���(�b)
    private float deltaTimeInMain = 0;

    public float GetDeltaTimeInMain => deltaTimeInMain;

    //���x��
    [NonSerialized]
    public int playerLevel = 1;

    //�|�����G�̐�
    [NonSerialized]
    public int deadEnemyMun = 0;

    //�t�B�[���h�̌o���l�̃��X�g
    public List<ItemExperienceValue> items;

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

        //�t���[�����[�g��ݒ�
        Application.targetFrameRate = 60;

        //�J�n���̃V�[��
        preSceneType = currentSceneType;

        items = new List<ItemExperienceValue>();
    }

    private void Update()
    {
        //���C���Q�[����ʂ̏ꍇ
        if (currentSceneType == SceneType.MainGame)
        {
            //���Ԃ��v��
            deltaTimeInMain += Time.deltaTime;
        }

        Debug.Log($"���݂̃V�[��:{currentSceneType}");
        Debug.Log($"�O�̃V�[��:{preSceneType}");

    }

    /// <summary>
    /// GameManager�C���X�^���X�ɃA�N�Z�X����
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                //�C���X�^���X���Z�b�g
                SetupInstance();
            }

            return instance;
        }
    }

    /// <summary>
    /// �C���X�^���X���Z�b�g����
    /// </summary>
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

    //���U���g�ł̒l�����Z�b�g����
    private void ResetResult()
    {
        deltaTimeInMain = 0;
        playerLevel = 1;
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
    }

    /// <summary>
    /// ���݂̃V�[����ύX����
    /// </summary>
    /// <param name="loadSceneName">���̃V�[����</param>
    public void ChangeSceneType(string nextSceneName)
    {
        preSceneType = currentSceneType;

        //�V�[�������猻�݂�SceneType��ύX

        if (nextSceneName == "TitleScene")
        {
            currentSceneType = SceneType.Title;
            ResetResult();
            SoundManager.uniqueInstance.PlayBgm("�^�C�g��");
        }
        else if (nextSceneName == "MainScene")
        {
            currentSceneType = SceneType.MainGame;
            SoundManager.uniqueInstance.PlayBgm("���C���Q�[��");
        }
        else if (nextSceneName == "ResultScene")
        {
            currentSceneType = SceneType.Result;
            SoundManager.uniqueInstance.PlayBgm("���U���g");
        }
        else
        {
            Debug.LogWarning($"{nextSceneName}�����݂̃V�[���ɕύX�ł��܂���");
        }
    }
}
