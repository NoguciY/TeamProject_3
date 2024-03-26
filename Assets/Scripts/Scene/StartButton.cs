using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement; // 忘れない！！

public class StartButton : MonoBehaviour
{
    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
        Time.timeScale = 1;
    }
}
