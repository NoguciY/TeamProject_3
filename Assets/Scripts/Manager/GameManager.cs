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

    //���C����ʂ̌o�ߎ���(�b)
    private float deltaTimeInMain;

    //�Q�b�^�[
    public float GetDeltaTimeInMain => deltaTimeInMain;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
        
        //�J�n���̃V�[��
        currentSceneType = SceneType.MainGame;
    }

    private void Update()
    {
        //���C���Q�[����ʂ̏ꍇ
        if(currentSceneType == SceneType.MainGame)
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

    /// <summary>
    /// ���݂̃V�[����ύX����
    /// </summary>
    /// <param name="nextSceneType">���̃V�[��</param>
    public void ChangeSceneType(SceneType nextSceneType)
    {
        currentSceneType = nextSceneType;
        Debug.Log($"���݂̃V�[���F{currentSceneType}");
    }

    /// <summary>
    /// ���݂̃V�[����ύX����
    /// </summary>
    /// <param name="loadSceneName">���̃V�[����</param>
    public void ChangeSceneType(string nextSceneName)
    {
        //�V�[��������SceneType�ɕϊ�
        if (nextSceneName == "TitleScene")
            currentSceneType = SceneType.Title;
        else if (nextSceneName == "MainScene")
            currentSceneType = SceneType.MainGame;
        else if(nextSceneName == "ResultScene")
            currentSceneType = SceneType.Result;
        else
            Debug.LogWarning($"{nextSceneName}�����݂̃V�[���ɕύX�ł��܂���");

        Debug.Log($"���݂̃V�[���F{currentSceneType}");
    }
}
