using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string dataFilePath;
    private readonly string keyWord = "Weapon";
    public StatsSO statsSO;
   

    private static DataManager _instance;
    public static DataManager Instance
    {
        get{ return _instance;}
    

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
       
        dataFilePath = $"{Application.persistentDataPath}/data.json";
        statsSO.Init(LoadUserData());

    }
    public void DeleteData()
    {
        // 데이터 파일 존재 여부 확인
        if (File.Exists(dataFilePath))
        {
            try
            {
                // 파일 삭제
                File.Delete(dataFilePath);
                Debug.Log("데이터 파일 삭제 성공: " + dataFilePath);

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
    }

    public void SaveData()
    {
        UserData uData = new()
        {
            statData = new StatData[statsSO.GetStats().Length]
        };

        int i = 0;
        foreach(Stat data in statsSO.GetStats())
        {
           uData.statData[i] = new()
           {
                key = data.key,
                textName = data.textName,
                floatScale  = data.floatScale,
                value = data.value.Value.ToString(),
                cost = data.cost.Value.ToString(),
                costRate = data.costRate,
                upgradeRate = data.upgradeRate,
                level = data.level.Value,



           };
           ++i;

        }

        uData.userLevel = statsSO.Level.Value;
        uData.userExp = statsSO.Exp.Value;
        uData.userCurHp = statsSO.CurHP.Value.ToString();
        uData.gold = statsSO.Gold.Value.ToString();
        // 데이터를 JSON으로 직렬화
        string jsonData = JsonUtility.ToJson(uData);

        // JSON 데이터를 파일로 저장
       // File.WriteAllText(dataFilePath, EncryptAndDecrypt(jsonData));
        File.WriteAllText(dataFilePath, (jsonData));
      
    }

    public UserData LoadUserData()
    {
      
        if (File.Exists(dataFilePath))
        {
            // 파일에서 JSON 데이터 읽기
            string jsonData = File.ReadAllText(dataFilePath);

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

        for(int i=0;i<data.Length;++i)
        {
           
            sb.Append((char)(data[i] ^ keyWord[i % keyWord.Length]));
        }
       
        return sb.ToString();
    }   


}

public class UserData
{

    public StatData[] statData;

    public int userLevel;
    public int userExp;
    public string userCurHp;
    public string gold;




}

[Serializable]
public class StatData
{
    

    public StatType key;
    public string textName;
    public int floatScale;

    public string value;

    public int level;

    public string cost;
    public float costRate;
    public float upgradeRate;


}

