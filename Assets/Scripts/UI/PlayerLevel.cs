using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;          //レベルテキスト

    private int currentLevel;
    private int oldLevel;

    public void CountLevel(int level)
    {
        currentLevel = level;
        if (currentLevel != oldLevel)
        {
            levelText.text = level.ToString() + "Lv";
        }
        oldLevel = currentLevel;
    }
}
