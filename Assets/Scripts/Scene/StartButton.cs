using UnityEngine;
using UnityEngine.SceneManagement; // 忘れない！！

public class StartButton : MonoBehaviour
{
    /// <summary>
    /// ボタンのクリックイベントに登録する
    /// 指定したシーンに切り替える
    /// </summary>
    /// <param name="loadSceneName">ロードするシーンの名前</param>
    public void LoadScene(string loadSceneName)
    {
        //シーンの遷移
        SceneManager.LoadScene(loadSceneName);
        GameManager.Instance.ChangeSceneType(loadSceneName);
        
        //タイトルからメインに遷移する場合は必要だけど
        //リザルトからタイトルに遷移する場合は必要ない?
        Time.timeScale = 1;
    }
}
