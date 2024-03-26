using UnityEngine;
using UnityEngine.SceneManagement; // ñYÇÍÇ»Ç¢ÅIÅI

public class StartButton : MonoBehaviour
{
    public void LoadScene(string MainScene)
    {
        SceneManager.LoadScene(MainScene);
    }
}
