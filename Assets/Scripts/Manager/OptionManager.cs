using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    //�I�v�V�����̎��
    private enum OptionType
    {
        Menu,   //���j���[
        Guide,  //�������
        Sound,  //���ʒ���
    }


    //�I�v�V�����p�l��
    [SerializeField]
    private GameObject optionPanel;

    //��������p�l��
    [SerializeField]
    private GameObject guideImage;

    //���ʒ��߃p�l��
    [SerializeField]
    private GameObject soundPanel;

    [SerializeField,Header("�I�v�V�����{�^��")]
    private Button optionButton;
    
    [SerializeField,Header("�I�v�V�����߂�{�^��")]
    private Button returnOptionButton;

    [SerializeField, Header("��������{�^��")]
    private Button guideButton;

    [SerializeField, Header("���ʒ��߃{�^��")]
    private Button soundButton;

    [SerializeField, Header("�Q�[���I���{�^��")]
    private Button quitGameButton;

    //���݂̃I�v�V�����̎��
    private OptionType currentOptionType;

    //returnOptionButton�ȊO�̃{�^�����i�[
    private Button[] optionButtons;

    //�I�v�V�����̍��ڐ�
    private const int OPTIONSNUM = 3;

    /// <summary>
    /// �I�v�V�����p�l���̃{�^���������̏�����ݒ肷��
    /// </summary>
    public void Initialize()
    {
        currentOptionType = OptionType.Menu;

        optionButtons = new Button[OPTIONSNUM] { guideButton, soundButton, quitGameButton};

        //�I�v�V������ʂ��J��
        optionButton.onClick.AddListener(
            () =>
            {
                //���ʉ����Đ�
                SoundManager.uniqueInstance.PlaySE("�{�^��");

                //�V�[���^�C�v���I�v�V�����ɕύX
                GameManager.Instance.PreSceneType = GameManager.Instance.CurrentSceneType;

                GameManager.Instance.CurrentSceneType = SceneType.Option;

                currentOptionType = OptionType.Menu;

                //�I�v�V������ʕ\��
                optionPanel.SetActive(true);
            });

        //��ʂ����
        returnOptionButton.onClick.AddListener(
            () => {
                //���ʉ����Đ�
                SoundManager.uniqueInstance.PlaySE("�{�^��");

                //���݂�OptionType�̉�ʂ����
                ClosePanel(currentOptionType);
            });

        //���������ʂ��J��
        guideButton.onClick.AddListener(
            () => {
                //���ʉ����Đ�
                SoundManager.uniqueInstance.PlaySE("�{�^��");

                currentOptionType = OptionType.Guide;

                //�߂�{�^���ȊO�𑀍�s�ɂ���
                for (int i = 0; i < OPTIONSNUM; i++)
                {
                    optionButtons[i].interactable = false;
                }

                //���������ʕ\��
                guideImage.SetActive(true);
            });

        //���ʒ��߉�ʂ��J��
        soundButton.onClick.AddListener(
            () => {
                //���ʉ����Đ�
                SoundManager.uniqueInstance.PlaySE("�{�^��");

                currentOptionType = OptionType.Sound;

                //�߂�{�^���ȊO�𑀍�s�ɂ���
                for (int i = 0; i < OPTIONSNUM; i++)
                {
                    optionButtons[i].interactable = false;
                }

                //���ʒ��߉�ʕ\��
                soundPanel.SetActive(true);
            });

        //�Q�[�����I������
        quitGameButton.onClick.AddListener(
            () => {
                //UnityEditor�Ńv���C���Ă���ꍇ
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;

                //�r���h�����Q�[�����v���C�����Ă���ꍇ
                #else
                    Application.Quit();
                #endif
            });
    }

    /// <summary>
    /// �I�v�V�����̎�ނŔ�\���ɂ����ʂ�I������
    /// </summary>
    /// <param name="optionType">�I�v�V�����̎��</param>
    private void ClosePanel(OptionType optionType)
    {
        //���j���[��ʂ̏ꍇ
        if (optionType == OptionType.Menu)
        {
            //�V�[���^�C�v��O��̃V�[���ɕύX
            GameManager.Instance.CurrentSceneType = GameManager.Instance.PreSceneType;

            //�O��̃V�[�����I�v�V�����ɕύX
            GameManager.Instance.PreSceneType = SceneType.Option;

            //�I�v�V������ʔ�\��
            optionPanel.SetActive(false);
        }
        //���������ʂ̏ꍇ
        else if (optionType == OptionType.Guide)
        {
            //�߂�{�^���ȊO�𑀍�\�ɂ���
            for (int i = 0; i < OPTIONSNUM; i++)
            {
                optionButtons[i].interactable = true;
            }

            //���������ʔ�\��
            guideImage.SetActive(false);
        }
        //���ʒ��߉�ʂ̏ꍇ
        else if (optionType == OptionType.Sound)
        {
            //�߂�{�^���ȊO�𑀍�\�ɂ���
            for (int i = 0; i < OPTIONSNUM; i++)
            {
                optionButtons[i].interactable = true;
            }

            //���ʒ��߉�ʔ�\��
            soundPanel.SetActive(false);
        }

        currentOptionType = OptionType.Menu;
    } 

    /// <summary>
    /// �I�v�V������ʂ̑���
    /// </summary>
    public void ControllOption()
    {
        //�I�v�V������ʈȊO�̏ꍇ
        if (GameManager.Instance.CurrentSceneType != SceneType.Option)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                optionButton.onClick.Invoke();
            }
        }
        //�I�v�V������ʂ̏ꍇ
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                returnOptionButton.onClick.Invoke();
            }
        }
    }
}
