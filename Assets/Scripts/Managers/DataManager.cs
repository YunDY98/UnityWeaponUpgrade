using System;
using System.IO;
using System.Numerics;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class DataManager : MonoBehaviour
{
    private string userDataFilePath;

    private readonly string keyWord = "Weapon";
    public StatsSO statsSO;


    private static DataManager _instance;
    public static DataManager Instance
    {
        get { return _instance; }


    }

    void Awake()
    {

        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;

            DontDestroyOnLoad(gameObject);
        }
        userDataFilePath = $"{Application.persistentDataPath}/data.json";

        statsSO.Init(LoadUserData());


    }

  


    public void LoadMission(Action<MissionList> onLoaded)
    {
        Addressables.LoadAssetAsync<TextAsset>("Assets/Mission/Mission.json").Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                string json = handle.Result.text;
                var list = JsonUtility.FromJson<MissionList>(json);
                print(json);
                onLoaded?.Invoke(list);
            }
            else
            {
                Debug.LogError("Addressable JSON 로드 실패!");
                onLoaded?.Invoke(null);
            }
        };
    }



    void OnApplicationQuit()
    {
        //SaveData();


    }
    public void DeleteData()
    {
        // 데이터 파일 존재 여부 확인
        if (File.Exists(userDataFilePath))
        {
            try
            {
                // 파일 삭제
                File.Delete(userDataFilePath);
                Debug.Log("데이터 파일 삭제 성공: " + userDataFilePath);

            }
            catch (System.Exception e)
            {
                Debug.LogError("데이터 파일 삭제 실패: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("삭제할 데이터 파일이 존재하지 않습니다.");
        }
        statsSO.Gold.Value = 0;
    }

    public void SaveData()
    {
        UserData uData = new()
        {
            statData = new StatData[statsSO.GetStats().Length]
        };


        uData.missionData.earnedGold =  MissionManager.Instance.earnedGold.ToString();
        uData.missionData.kill = MissionManager.Instance.kill;
        uData.missionData.missionID = MissionManager.Instance.missionID;

        int i = 0;
        foreach (Stat data in statsSO.GetStats())
        {
            uData.statData[i] = new()
            {
                // key = data.key,
                // textName = data.textName,
                // floatScale  = data.floatScale,
                // baseValue = data.baseValue,
                // baseCost = data.baseCost,
                // costRate = data.costRate,
                // upgradeRate = data.upgradeRate,
                level = data.level.Value,

                // maxLevel = data.maxLevel,




            };
            ++i;

        }
        uData.userLevel = statsSO.Level.Value;
        uData.userExp = statsSO.Exp.Value;

        uData.gold = statsSO.Gold.Value.ToString();
        // 데이터를 JSON으로 직렬화
        string jsonData = JsonUtility.ToJson(uData);

        // JSON 데이터를 파일로 저장
        // File.WriteAllText(userDataFilePath, EncryptAndDecrypt(jsonData));
        File.WriteAllText(userDataFilePath, (jsonData));

    }

    public UserData LoadUserData()
    {


        if (File.Exists(userDataFilePath))
        {
            // 파일에서 JSON 데이터 읽기
            string jsonData = File.ReadAllText(userDataFilePath);

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                Debug.LogWarning("데이터 파일은 존재하지만 내용이 없습니다.");
                return null;
            }


            //return JsonUtility.FromJson<UserData>(EncryptAndDecrypt(jsonData));
            return JsonUtility.FromJson<UserData>((jsonData));


        }
        else
        {
            Debug.LogError("데이터 파일이 존재하지 않습니다.");
            return null;
        }
    }

    private string EncryptAndDecrypt(string data)
    {

        StringBuilder sb = new();

        for (int i = 0; i < data.Length; ++i)
        {

            sb.Append((char)(data[i] ^ keyWord[i % keyWord.Length]));
        }

        return sb.ToString();
    }


}

public class UserData
{

    public StatData[] statData;

    public int userLevel = 1;
    public int userExp = 0;
    public string gold = " ";


    public UserMissionData missionData = new();




}

[Serializable]
public class StatData
{


    // public StatType key;
    // public string textName;
    // public int floatScale;

    // public int baseValue;

    public int level;
    // public int maxLevel;
    // public int baseCost;
    // public float costRate;
    // public float upgradeRate;


}
[Serializable]
public class MissionList
{
    public MissionData[] missions;
}

[Serializable]
public class MissionData
{
    public int id = 1;
    public string type = "";
    public string description = "";
    public string goal = "0";

    public MissionReward rewards = new();
}

[Serializable]
public class MissionReward
{
    public string type = "";      // 예: Gold, Exp, Item 등
    public int amount = 0;
}

[Serializable]
public class UserMissionData
{
    public int missionID = 1;
    public int kill = 0;
    public string earnedGold = "0";


}
