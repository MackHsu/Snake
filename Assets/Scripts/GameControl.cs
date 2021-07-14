using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{

    [Tooltip("玩家")]
    public GameObject player;
    [Tooltip("初始尾巴数量")]
    public int startTrailNumber;
    [Tooltip("尾巴Prefab")]
    public GameObject trailPrefab;
    [Tooltip("每节蛇身之间的空隙")]
    public float distance;
    [Tooltip("基础速度")]
    public float baseSpeed;
    [Tooltip("加速时间")]
    public float accelerateTime;
    [Tooltip("拾取物Prefab")]
    public GameObject pickablePrefab;
    [Tooltip("拾取物level1权重")]
    public float weight1;
    [Tooltip("拾取物level2权重")]
    public float weight2;
    [Tooltip("拾取物level3权重")]
    public float weight3;
    [Tooltip("玩家名")]
    public Text playerName;
    [Tooltip("分数")]
    public Text score;
    [Tooltip("游戏结束ui")]
    public GameObject endUI;

    // 蛇头历史位置列表（循环数组）
    private static List<Vector3> posList;
    // posList容量
    private static int MAX = 50000;
    // posList当前头部下标
    private static int index;

    // 尾巴列表
    private static List<GameObject> Trails;
    // 尾巴前进的位置在posList中的下标
    private static List<int> indexList;

    // 游戏进行中
    public static bool flag = true;

    private static GameManager gm;

    void Start()
    {
        flag = true;
        gm = GameManager.GetManager();
        playerName.text = gm.CurrentRecord.playerName;
        score.text = gm.CurrentRecord.score.ToString();
        player.GetComponent<PlayerControl>().speed = baseSpeed;

        Trails = new List<GameObject>();
        posList = new List<Vector3>(MAX);

        // 将蛇尾的初始位置添加进列表
        posList.Add(player.transform.position - new Vector3(distance, 0, 0));
        index = 0;

        indexList = new List<int>();

        // 初始化尾巴
        Vector3 formerPos = player.transform.position;
        for (int i = 0; i < startTrailNumber; i++)
        {
            GameObject trail = GameObject.Instantiate(trailPrefab, player.transform.position - new Vector3(distance, 0, 0), Quaternion.identity);
            Trails.Add(trail);
            indexList.Add(0);
            formerPos = trail.transform.position;
        }

        // 生成拾取物
        GeneratePickable();
    }

    void FixedUpdate()
    {
        if (!flag) return;

        // 记录蛇头的历史位置
        if (posList.Count < MAX) posList.Add(player.transform.position);
        else
        {
            posList[index] = player.transform.position;
            index = (index + 1) % MAX;
        }

        // 尾巴跟随
        for (int i = 0; i < Trails.Count; i++)
        {
            GameObject trail = Trails[i];
            if (i == 0)
            {
                if ((trail.transform.position - player.transform.position).magnitude > distance)
                    while ((posList[indexList[i]] - player.transform.position).magnitude > distance)
                    {
                        trail.transform.position = posList[indexList[i]];
                        indexList[i] = (indexList[i] + 1) % MAX;
                    }
            }
            else
            {
                GameObject former = Trails[i - 1];
                if ((trail.transform.position - former.transform.position).magnitude > distance)
                    while ((posList[indexList[i]] - former.transform.position).magnitude > distance)
                    {
                        trail.transform.position = posList[indexList[i]];
                        indexList[i] = (indexList[i] + 1) % MAX;
                    }
            }
        }
    }

    // 拾取
    public void Pick(Pickable p)
    {
        gm.CurrentRecord.score += p.GetScore();
        score.text = gm.CurrentRecord.score.ToString();
        AddLength(p.getLength());
        StopCoroutine("Accelerate"); // 覆盖当前的加速状态
        StartCoroutine("Accelerate", p.getAccelerate());
        GeneratePickable();
    }

    private void AddLength(int length)
    {
        for (int i = 0; i < length; i++)
        {
            Vector3 pos = Trails[Trails.Count - 1].transform.position;
            int index = indexList[Trails.Count - 1];
            GameObject trail = GameObject.Instantiate(trailPrefab, pos, Quaternion.identity);
            Trails.Add(trail);
            indexList.Add(index);
        }
    }

    IEnumerator Accelerate(float a)
    {
        player.GetComponent<PlayerControl>().speed = baseSpeed * a;
        yield return new WaitForSeconds(accelerateTime);
        player.GetComponent<PlayerControl>().speed = baseSpeed;
    }

    private void GeneratePickable()
    {
        // 生成位置
        Vector3 pos = new Vector3();
        while (true)
        {
            pos = new Vector3(Random.Range(-90, 90), 7.5F, Random.Range(-90, 90));
            if ((pos - player.transform.position).magnitude > distance) break;
        }

        GameObject pickable = GameObject.Instantiate(pickablePrefab, pos, Quaternion.identity);

        // 生成种类
        float total = weight1 + weight2 + weight3;
        float ran = Random.Range(0, total);
        if (ran < weight1) pickable.GetComponent<Pickable>().setLevel(1);
        else if (ran < weight1 + weight2) pickable.GetComponent<Pickable>().setLevel(2);
        else pickable.GetComponent<Pickable>().setLevel(3);

    }

    public void EndGame()
    {
        flag = false;
        gm.AddRecord();
        endUI.SetActive(true);
        StartCoroutine("BackIn3Secs");
    }

    IEnumerator BackIn3Secs()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Welcome");
    }
}
