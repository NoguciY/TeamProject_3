using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    //オプションの種類
    private enum OptionType
    {
        Menu,   //メニュー
        Guide,  //操作説明
        Sound,  //音量調節
    }


    //オプションパネル
    [SerializeField]
    private GameObject optionPanel;

    //操作説明パネル
    [SerializeField]
    private GameObject guideImage;

    //音量調節パネル
    [SerializeField]
    private GameObject soundPanel;

    [SerializeField,Header("オプションボタン")]
    private Button optionButton;
    
    [SerializeField,Header("オプション戻るボタン")]
    private Button returnOptionButton;

    [SerializeField, Header("操作説明ボタン")]
    private Button guideButton;

    [SerializeField, Header("音量調節ボタン")]
    private Button soundButton;

    [SerializeField, Header("ゲーム終了ボタン")]
    private Button quitGameButton;

    //現在のオプションの種類
    private OptionType currentOptionType;

    //returnOptionButton以外のボタンを格納
    private Button[] optionButtons;

    //オプションの項目数
    private const int OPTIONSNUM = 3;

    /// <summary>
    /// オプションパネルのボタン押下時の処理を設定する
    /// </summary>
    public void Initialize()
    {
        currentOptionType = OptionType.Menu;

        optionButtons = new Button[OPTIONSNUM] { guideButton, soundButton, quitGameButton};

        //オプション画面を開く
        optionButton.onClick.AddListener(
            () =>
            {
                //効果音を再生
                SoundManager.uniqueInstance.PlaySE("ボタン");

                //シーンタイプをオプションに変更
                GameManager.Instance.PreSceneType = GameManager.Instance.CurrentSceneType;

                GameManager.Instance.CurrentSceneType = SceneType.Option;

                currentOptionType = OptionType.Menu;

                //オプション画面表示
                optionPanel.SetActive(true);
            });

        //画面を閉じる
        returnOptionButton.onClick.AddListener(
            () => {
                //効果音を再生
                SoundManager.uniqueInstance.PlaySE("ボタン");

                //現在のOptionTypeの画面を閉じる
                ClosePanel(currentOptionType);
            });

        //操作説明画面を開く
        guideButton.onClick.AddListener(
            () => {
                //効果音を再生
                SoundManager.uniqueInstance.PlaySE("ボタン");

                currentOptionType = OptionType.Guide;

                //戻るボタン以外を操作不可にする
                for (int i = 0; i < OPTIONSNUM; i++)
                {
                    optionButtons[i].interactable = false;
                }

                //操作説明画面表示
                guideImage.SetActive(true);
            });

        //音量調節画面を開く
        soundButton.onClick.AddListener(
            () => {
                //効果音を再生
                SoundManager.uniqueInstance.PlaySE("ボタン");

                currentOptionType = OptionType.Sound;

                //戻るボタン以外を操作不可にする
                for (int i = 0; i < OPTIONSNUM; i++)
                {
                    optionButtons[i].interactable = false;
                }

                //音量調節画面表示
                soundPanel.SetActive(true);
            });

        //ゲームを終了する
        quitGameButton.onClick.AddListener(
            () => {
                //UnityEditorでプレイしている場合
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;

                //ビルドしたゲームをプレイしいている場合
                #else
                    Application.Quit();
                #endif
            });
    }

    /// <summary>
    /// オプションの種類で非表示にする画面を選択する
    /// </summary>
    /// <param name="optionType">オプションの種類</param>
    private void ClosePanel(OptionType optionType)
    {
        //メニュー画面の場合
        if (optionType == OptionType.Menu)
        {
            //シーンタイプを前回のシーンに変更
            GameManager.Instance.CurrentSceneType = GameManager.Instance.PreSceneType;

            //前回のシーンをオプションに変更
            GameManager.Instance.PreSceneType = SceneType.Option;

            //オプション画面非表示
            optionPanel.SetActive(false);
        }
        //操作説明画面の場合
        else if (optionType == OptionType.Guide)
        {
            //戻るボタン以外を操作可能にする
            for (int i = 0; i < OPTIONSNUM; i++)
            {
                optionButtons[i].interactable = true;
            }

            //操作説明画面非表示
            guideImage.SetActive(false);
        }
        //音量調節画面の場合
        else if (optionType == OptionType.Sound)
        {
            //戻るボタン以外を操作可能にする
            for (int i = 0; i < OPTIONSNUM; i++)
            {
                optionButtons[i].interactable = true;
            }

            //音量調節画面非表示
            soundPanel.SetActive(false);
        }

        currentOptionType = OptionType.Menu;
    } 

    /// <summary>
    /// オプション画面の操作
    /// </summary>
    public void ControllOption()
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
                returnOptionButton.onClick.Invoke();
            }
        }
    }
}
