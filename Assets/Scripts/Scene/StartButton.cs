using UnityEngine;
using UnityEngine.SceneManagement; // 忘れない！！

public class StartButton : MonoBehaviour
{
    public void LoadScene(string MainScene)
    {
        SceneManager.LoadScene(MainScene);
    }
}
