using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement; // �Y��Ȃ��I�I

public class StartButton : MonoBehaviour
{
    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
