using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    private string name;
    private string description;
    private bool isClear;

    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsAchieved { get; set; }

    public Achievement(string description, bool isAchieved)
    {
        Description = description;
        IsAchieved = isAchieved;
    }
}


public class AchievementManager : MonoBehaviour
{
    public Achievement achievementData;

    Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>
    {
        {"퇴실 도전", new Achievement("퇴실 시도를 처음 해봤다.", false)},
        {"아이쿠!", new Achievement("장애물에 처음으로 부딪혔다.", false)},
        {"500", new Achievement("점수 500점을 돌파했다.", false)},
    };
    
    void Start()
    {
        
    }

    // isAchieved =  true
    public void Achieve()
    {
        // 업적 달성 UIpanel 만들어서 SetActive()
        // isAchieved?
    }


    // Json
    //void SaveAchievementData()
    //{
    //    string saveData = JsonUtility.ToJson(achievementData);
    //    File.WriteAllText(Application.persistentDataPath + "/AchievementData.txt", saveData);
    //}
    
    //void LoadAchievementData()
    //{
    //    string loadData = File.ReadAllText(Application.persistentDataPath + "/AchievementData.txt");
    //    achievementData = JsonUtility.FromJson<Achievement>(loadData);
    //}
}
