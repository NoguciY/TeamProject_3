public interface IGettableItem
{
    /// <summary>
    /// 経験値を取得する
    /// </summary>
    /// <param name="experienceValue">経験値</param>
    void GetExperienceValue(float experienceValue);

    /// <summary>
    /// 回復する
    /// </summary>
    /// <param name="healValue">回復量</param>
    void Heal(float healValue);
}
