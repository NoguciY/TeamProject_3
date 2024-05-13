using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerLevelUp : MonoBehaviour
{
    //経験値コンポーネント
    [SerializeField]
    private ExperienceValue experienceValue;

    //最大レベル
    private int maxLevel = 300;

    //現在のレベル
    [SerializeField]
    private int level;

    //現在の経験値
    private int exp;

    //レベルアップに必要な経験値
    private int needExp;


    //ゲッター
    public int GetLevel => level;
    public int GetExp => exp;
    public int GetNeedExp => needExp;

    //初期化
    public void InitLevel()
    {
        level = 1;
        exp = 0;

        //各レベルに必要な経験値を計算
        experienceValue.CalNeedExperience(maxLevel);

        //現在のレベルアップに必要な経験値を取得
        needExp = experienceValue.GetNeedExp(level + 1);
    }

    //経験値を得る
    public void AddExp(int exp)
    {
        this.exp += exp;
    }

    //レベルアップ
    public void LevelUp(UnityEvent levelUpEvent)
    {
        //経験値がレベルアップに必要な経験値以上になった場合
        if (exp >= needExp)
        {
            //レベルアップ
            level++;
            exp = 0;
            needExp = experienceValue.GetNeedExp(level + 1);
            Debug.Log($"レベルアップ！\n 現在のレベル：{level}");

            //イベント(パネルの表示やエフェクトなど)を実行
            levelUpEvent.Invoke();
        }
    }
}
