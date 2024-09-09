using UnityEngine;
using UnityEngine.SceneManagement; // �Y��Ȃ��I�I

public class StartButton : MonoBehaviour
{
    /// <summary>
    /// �{�^���̃N���b�N�C�x���g�ɓo�^����
    /// �w�肵���V�[���ɐ؂�ւ���
    /// </summary>
    /// <param name="loadSceneName">���[�h����V�[���̖��O</param>
    public void LoadScene(string loadSceneName)
    {
        //�V�[���̑J��
        SceneManager.LoadScene(loadSceneName);
        GameManager.Instance.ChangeSceneType(loadSceneName);
        
        //�^�C�g�����烁�C���ɑJ�ڂ���ꍇ�͕K�v������
        //���U���g����^�C�g���ɑJ�ڂ���ꍇ�͕K�v�Ȃ�?
        Time.timeScale = 1;
    }
}
