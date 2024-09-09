using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    //レベルテキスト
    [SerializeField]
    private TextMeshProUGUI levelText;          

    //現在のレベル
    private int currentLevel;

    //前のレベル
    private int beforeLevel;

    /// <summary>
    /// レベルが変化した場合、現在のレベルにテキストを書き換える
    /// </summary>
    /// <param name="level">レベル</param>
    public void CountLevel(int level)
    {
        currentLevel = level;

        if (currentLevel != beforeLevel)
        {
            levelText.text = level.ToString() + "Lv";
        }
        beforeLevel = currentLevel;
    }
}
