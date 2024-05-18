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
    private static GameManager instance;

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
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //���C���Q�[����ʂ̏ꍇ

        //���Ԃ��v��
        deltaTimeInMain += Time.deltaTime;
    }

    //GameManager�C���X�^���X�ɃA�N�Z�X����
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
}
