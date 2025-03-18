using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// 개별 업적 클래스
/// </summary>
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

/// <summary>
/// 개별 업적을 담을 리스트 (Json 직렬화)
/// </summary>
[System.Serializable]
public class AchievementData
{
    public List<Achievement> achievementData;
}

/// <summary>
/// 업적 로직 관리하는 매니저
/// </summary>
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    public List<Achievement> achievementData = new List<Achievement>();


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

    /// <summary>
    /// 게임 시작 시 업적 데이터 로드
    /// 경로에 파일이 존재하면 그 데이터 불러오기
    /// 경로에 파일이 없다면 업적 리스트에 업적 추가 후 저장 / 게임 시작 업적 달성
    /// </summary>
    void Start()
    {
        DeleteData();       // test용. 마무리할 때 주석 처리 후 커밋

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

    /// <summary>
    /// 리스트에 업적 추가
    /// </summary>
    void AddAchievenments()
    {
        achievementData.Add(new Achievement("Beginner", "Play the game for the first time.", false));   // 처음 시작
        achievementData.Add(new Achievement("Crash!", "First collision with an obstacle", false));      // 장애물 충돌
        achievementData.Add(new Achievement("Good Runner", "Earn 1,000 points.", false));               // 점수
        achievementData.Add(new Achievement("Item Collector", "Obtain Items 5 Times.", false));         // 아이템 획득
    }

    /// <summary>
    /// 처음 달성하면 UI 표시 후 저장
    /// 이미 달성한거면 리턴
    /// </summary>
    /// <param name="listIdx">추가한 리스트의 인덱스</param>
    public void Achieve(int listIdx)
    {
        if (achievementData[listIdx].IsAchieved)
        {
            return;
        }
        else
        {
            //StartCoroutine(UIManager.Instance.SetAchievementUI(listIdx));     // UIManager에 옮긴 후 주석 해제
            SoundManager.instance.PlaySFX(SFX.ACHIEVED);
            achievementData[listIdx].IsAchieved = true;
            SaveAchievementData();
        }
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    void SaveAchievementData()
    {
        AchievementData data = new AchievementData { achievementData = achievementData };
        string saveData = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/AchievementData.txt", saveData);

        Debug.Log("저장 완료.\n" + saveData);
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    void LoadAchievementData()
    {
        string loadData = File.ReadAllText(Application.persistentDataPath + "/AchievementData.txt");
        AchievementData data = JsonUtility.FromJson<AchievementData>(loadData);
        achievementData = data.achievementData;

        Debug.Log("로드 완료.\n" + loadData);
    }

    /// <summary>
    /// 저장된 데이터 삭제 (테스트용)
    /// </summary>
    void DeleteData()
    {
        string jsonData = Application.persistentDataPath + "/AchievementData.txt";
        if (File.Exists(jsonData))
            File.Delete(jsonData);
    }
}