using UnityEngine;
using UnityEngine.Events;


public class PlayerLevelUp : MonoBehaviour
{
    //経験値コンポーネント
    [SerializeField]
    private ExperienceValue experienceValue;

    //最大レベル
    private int maxLevel;

    //現在のレベル
    [SerializeField]
    private int level;

    //現在の経験値
    private float currentExperienceValue;

    //レベルアップに必要な経験値
    private float needExperienceValue;


    //ゲッター
    public int GetLevel => level;
    public float GetExperienceValue => currentExperienceValue;
    public float GetNeedExperienceValue => needExperienceValue;

    /// <summary>
    /// 初期化
    /// </summary>
    public void InitLevel()
    {
        level = 1;
        currentExperienceValue = 0;
        maxLevel = 300;

        //各レベルに必要な経験値を計算
        experienceValue.SetNeedExperience(maxLevel);

        //現在のレベルアップに必要な経験値を取得
        needExperienceValue = experienceValue.GetNeedExperienceValue(level);
    }

    /// <summary>
    /// 経験値を得る
    /// </summary>
    /// <param name="experienceValue">経験値</param>
    public void AddExperienceValue(float experienceValue)
    {
        currentExperienceValue += experienceValue;
        Debug.Log($"現在の経験値：{currentExperienceValue}");
    }

    /// <summary>
    /// レベルアップ
    /// </summary>
    /// <param name="normalLevelUpEvent">通常のレベルアップイベント</param>
    /// <param name="addNewBombEvent">新しい爆弾を追加するイベント</param>
    /// <param name="addNewBombLevel">新しい爆弾を追加するレベル</param>
    public void LevelUp(UnityEvent normalLevelUpEvent, UnityEvent addNewBombEvent, int addNewBombLevel)
    {
        //経験値がレベルアップに必要な経験値以上になった場合
        if (currentExperienceValue >= needExperienceValue)
        {
            level++;
            currentExperienceValue -= needExperienceValue;
            needExperienceValue = experienceValue.GetNeedExperienceValue(level);

            Debug.Log($"レベルアップ！\n 現在のレベル：{ level }");
            Debug.Log($"必要な経験値：{ needExperienceValue }");

            //新しい爆弾を追加するイベントを実行
            if (level == addNewBombLevel)
            {
                addNewBombEvent.Invoke();
            }
            //通常のレベルアップイベントを実行
            else
            {
                normalLevelUpEvent.Invoke();
            }
        }
    }
}
