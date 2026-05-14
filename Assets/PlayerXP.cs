using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 1;

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("XP: " + currentXP + "/" + xpToNextLevel);

        if (currentXP >= xpToNextLevel)
            LevelUp();
    }

    void LevelUp()
    {
        level++;
        currentXP = 0;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        SoundManager.instance?.PlayLevelUp();
        if (LevelUpManager.instance != null)
            LevelUpManager.instance.ShowLevelUp();
    }
}
