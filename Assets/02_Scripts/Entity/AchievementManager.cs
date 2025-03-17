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

    [SerializeField] private GameObject achievePanel;
    [SerializeField] private TextMeshProUGUI achieveTitleTxt;
    [SerializeField] private TextMeshProUGUI achieveDescTxt;


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

    // 게임 처음 시작하면 리스트 추가 + 저장 + 스타트 업적 달성. 재시작하면 로드만
    void Start()
    {
        DeleteData();       // test

        string jsonPath = Application.persistentDataPath + "/AchievementData.txt";
        if (File.Exists(jsonPath))
        {
            LoadAchievementData();
        }
        else
        {
            AddAchievenments();
            SaveAchievementData();
        }

        // test
        Debug.Log(Application.persistentDataPath);
        Achieve(0);
    }

    // 업적 리스트 추가
    void AddAchievenments() 
    {
        achievementData.Add(new Achievement("Beginner", "Play the game for the first time.", false));   // 처음 시작
        achievementData.Add(new Achievement("Crash!", "First collision with an obstacle", false));      // 장애물 충돌
        achievementData.Add(new Achievement("Good Runner", "Earn 500 points.", false));               // 점수
        achievementData.Add(new Achievement("Item Collector", "Obtain Items 5 Times.", false));         // 아이템 획득
    }

    // 업적 달성했을 때. 이미 달성해봤다면 패스
    public void Achieve(int listIdx)
    {
        if (achievementData[listIdx].IsAchieved)
        {
            return;
        }
        else
        {
            StartCoroutine(SetAchievementUI(listIdx));
            SoundManager.instance.PlaySFX(SFX.ACHIEVED);
            achievementData[listIdx].IsAchieved = true;
            SaveAchievementData();
        }
    }

    // 업적 UI 3초 활성화
    private IEnumerator SetAchievementUI(int listIdx)
    {
        achievePanel.gameObject.SetActive(true);
        achieveTitleTxt.text = achievementData[listIdx].Name.ToString();
        achieveDescTxt.text = achievementData[listIdx].Description.ToString();

        yield return new WaitForSeconds(3f);

        achievePanel.gameObject.SetActive(false);
    }

    // JSON 저장/ 로드
    void SaveAchievementData()
    {
        AchievementData data = new AchievementData { achievementData = achievementData };
        string saveData = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/AchievementData.txt", saveData);

        Debug.Log("저장 완료.\n" + saveData);
    }

    void LoadAchievementData()
    {
        string loadData = File.ReadAllText(Application.persistentDataPath + "/AchievementData.txt");
        AchievementData data = JsonUtility.FromJson<AchievementData>(loadData);
        achievementData = data.achievementData;

        Debug.Log("로드 완료.\n" + loadData);
    }

    // TEST METHOD
    void DeleteData()
    {
        string jsonData = Application.persistentDataPath + "/AchievementData.txt";
        if (File.Exists(jsonData))
            File.Delete(jsonData);
    }
}
