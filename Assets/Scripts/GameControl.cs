using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // 蛇头历史位置列表（循环数组）
    private List<Vector3> posList;
    // posList容量
    private int MAX = 100;
    // posList当前尾部下标
    private int index;

    // 尾巴列表
    private List<GameObject> Trails;
    // 尾巴前进的位置在posList中的下标
    private List<int> indexList;

    void Start()
    {
        Trails = new List<GameObject>();
        posList = new List<Vector3>(MAX);

        // 将蛇尾的初始位置添加进列表
        posList.Add(player.transform.position - player.transform.position - new Vector3(distance, 0, 0));
        index = 1;

        indexList = new List<int>();

        // 初始化尾巴
        for (int i = 0; i < startTrailNumber; i++)
        {
            GameObject trail = GameObject.Instantiate(trailPrefab, player.transform.position - new Vector3(distance, 0, 0), Quaternion.identity);
            Trails.Add(trail);
            indexList.Add(0);
        }
    }

    void FixedUpdate()
    {
        // 记录蛇头的历史位置
        if (posList.Count < MAX) posList.Add(player.transform.position);
        else posList[index] = player.transform.position;
        index = (index + 1) % MAX;

        // 尾巴跟随
        for (int i = 0; i < Trails.Count; i++)
        {
            GameObject trail = Trails[i];
            if (i == 0)
            {
                // 第一节蛇尾
                while ((posList[indexList[i]] - player.transform.position).magnitude > distance)
                {
                    trail.transform.position = posList[indexList[i]];
                    indexList[i] = (indexList[i] + 1) % MAX;
                }
            }
            else
            {
                GameObject former = Trails[i - 1];
                while ((posList[indexList[i]] - former.transform.position).magnitude > distance)
                {
                    trail.transform.position = posList[indexList[i]];
                    indexList[i] = (indexList[i] + 1) % MAX;
                }
            }
        }
    }
}
