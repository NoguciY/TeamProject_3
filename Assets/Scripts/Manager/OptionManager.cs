using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject optionPanel;

    [SerializeField,Header("�I�v�V�����{�^��")]
    private Button optionButton;
    
    [SerializeField,Header("�I�v�V�����߂�{�^��")]
    private Button finishOptionButton;

    private void Start()
    {
        //�I�v�V�����{�^��
        optionButton.onClick.AddListener(
            () =>
            {
                //���ʉ����Đ�
                SoundManager.uniqueInstance.Play("�{�^��");

                //�V�[���^�C�v���I�v�V�����ɕύX
                GameManager.Instance.PreSceneType = GameManager.Instance.CurrentSceneType;

                GameManager.Instance.CurrentSceneType = SceneType.Option;

                //�I�v�V������ʕ\��
                optionPanel.SetActive(true);
            });

        //�I�v�V������ʕ���{�^��
        finishOptionButton.onClick.AddListener(
            () => {
                //���ʉ����Đ�
                SoundManager.uniqueInstance.Play("�{�^��");

                //�V�[���^�C�v��O��̃V�[���ɕύX
                GameManager.Instance.CurrentSceneType = GameManager.Instance.PreSceneType;
                
                //�O��̃V�[�����I�v�V�����ɕύX
                GameManager.Instance.PreSceneType = SceneType.Option;

                //�I�v�V������ʔ�\��
                optionPanel.SetActive(false); 
            });
    }
    private void Update()
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
                finishOptionButton.onClick.Invoke();
            }
        }
    }
}
