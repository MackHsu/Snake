using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager gm;

    // 玩家名、分数的记录
    private List<Record> records;
    private Record currentRecord;
    // 记录文件路径
    private string filePath;

    public Record CurrentRecord { get => currentRecord; set => currentRecord = value; }
    public List<Record> Records { get => records; set => records = value; }

    public static GameManager GetManager() { return gm; }

    private void Awake()
    {
        if (gm != null)
        {
            Destroy(gameObject);
            return;
        }
        gm = this;
        DontDestroyOnLoad(gameObject);

        Records = new List<Record>();
        currentRecord = new Record();

        filePath = Application.persistentDataPath + '/' + "scoreList.json";

        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                string json;
                while ((json = sr.ReadLine()) != null)
                {
                    Records.Add(JsonUtility.FromJson<Record>(json));
                }
            }
        }
    }

    public void AddRecord()
    {
        Records.Add(CurrentRecord);

        // 写前10名
        using (FileStream fs = new FileStream(filePath, FileMode.Truncate, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                Records.Sort();
                int rank = 1; // 名次
                float temp = Records[0].score; // 前一名的分数
                int i = 0;
                for (; i < Records.Count; i++)
                {
                    if (Records[i].score < temp)
                    {
                        rank++;
                    }
                    if (rank > 10) break;
                    sw.WriteLine(JsonUtility.ToJson(Records[i]));

                    temp = Records[i].score;
                }

                // 舍弃10名之后的记录
                while (i < Records.Count)
                    Records.RemoveAt(i);
            }
        }
    }
}
