using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject optionPanel;

    [SerializeField,Header("オプションボタン")]
    private Button optionButton;
    
    [SerializeField,Header("オプション戻るボタン")]
    private Button finishOptionButton;

    private void Start()
    {
        //オプションボタン
        optionButton.onClick.AddListener(
            () =>
            {
                //効果音を再生
                SoundManager.uniqueInstance.Play("ボタン");

                //シーンタイプをオプションに変更
                GameManager.Instance.PreSceneType = GameManager.Instance.CurrentSceneType;

                GameManager.Instance.CurrentSceneType = SceneType.Option;

                //オプション画面表示
                optionPanel.SetActive(true);
            });

        //オプション画面閉じるボタン
        finishOptionButton.onClick.AddListener(
            () => {
                //効果音を再生
                SoundManager.uniqueInstance.Play("ボタン");

                //シーンタイプを前回のシーンに変更
                GameManager.Instance.CurrentSceneType = GameManager.Instance.PreSceneType;
                
                //前回のシーンをオプションに変更
                GameManager.Instance.PreSceneType = SceneType.Option;

                //オプション画面非表示
                optionPanel.SetActive(false); 
            });
    }
    private void Update()
    {
        //オプション画面以外の場合
        if (GameManager.Instance.CurrentSceneType != SceneType.Option)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                optionButton.onClick.Invoke();
            }
        }
        //オプション画面の場合
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                finishOptionButton.onClick.Invoke();
            }
        }
    }
}
