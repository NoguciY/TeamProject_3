using UnityEngine;
using UnityEngine.SceneManagement; // �Y��Ȃ��I�I

public class StartButton : MonoBehaviour
{
    public void LoadScene(string MainScene)
    {
        SceneManager.LoadScene(MainScene);
    }
}
