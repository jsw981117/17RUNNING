using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;


// 개별 업적
[System.Serializable]
public class Achievement
{
    public string Name;
    public string Description;
    public bool IsAchieved;

    public Achievement(string name, string description, bool isAchieved)
    {
        Name = name;
        Description = description;
        IsAchieved = isAchieved;
    }
}

// 개별 업적 담을 리스트
[System.Serializable]
public class AchievementData
{
    public List<Achievement> achievementData;
}


// 업적 로직
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    public List<Achievement> achievementData = new List<Achievement>();

    [SerializeField] private TextMeshProUGUI achieveText;       // temp


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        AddAchievenments();    // start될 때마다 add되면 X → 테스트해보면서 방어 로직

        //SaveAchievementData();
        //LoadAchievementData();
    }

    void AddAchievenments()
    {
        achievementData.Add(new Achievement("게임 스타트", "게임을 처음 시작했다.", false));
        achievementData.Add(new Achievement("장애물", "장애물에 처음으로 부딪혔다.", false));
        achievementData.Add(new Achievement("1000", "점수 1000점을 돌파했다.", false));
    }

    public void Achieve()
    {
        // 업적 달성 어떻게 체크할건지?
        // 업적 달성 UIpanel 만들어서 SetActive()
        // bool isAchieved?
    }

    // Json
    void SaveAchievementData()
    {
        AchievementData data = new AchievementData { achievementData = achievementData };
        string saveData = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/AchievementData.txt", saveData);

        Debug.Log(Application.persistentDataPath);
        Debug.Log("저장 완료.\n" + saveData);
    }

    void LoadAchievementData()
    {
        string loadData = File.ReadAllText(Application.persistentDataPath + "/AchievementData.txt");
        achievementData = JsonUtility.FromJson<List<Achievement>>(loadData);

        Debug.Log("로드 완료.\n" + loadData);
    }

    void DeleteAchievementData()    // temp. test
    {
        string jsonPath = Application.persistentDataPath + "/AchievementData.txt";
        if (File.Exists(jsonPath))
            File.Delete(jsonPath);
    }
}
