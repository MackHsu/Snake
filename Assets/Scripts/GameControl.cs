using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{

    [Tooltip("���")]
    public GameObject player;
    [Tooltip("��ʼβ������")]
    public int startTrailNumber;
    [Tooltip("β��Prefab")]
    public GameObject trailPrefab;
    [Tooltip("ÿ������֮��Ŀ�϶")]
    public float distance;

    // ��ͷ��ʷλ���б�ѭ�����飩
    private List<Vector3> posList;
    // posList����
    private int MAX = 100;
    // posList��ǰβ���±�
    private int index;

    // β���б�
    private List<GameObject> Trails;
    // β��ǰ����λ����posList�е��±�
    private List<int> indexList;

    void Start()
    {
        Trails = new List<GameObject>();
        posList = new List<Vector3>(MAX);

        // ����β�ĳ�ʼλ����ӽ��б�
        posList.Add(player.transform.position - player.transform.position - new Vector3(distance, 0, 0));
        index = 1;

        indexList = new List<int>();

        // ��ʼ��β��
        for (int i = 0; i < startTrailNumber; i++)
        {
            GameObject trail = GameObject.Instantiate(trailPrefab, player.transform.position - new Vector3(distance, 0, 0), Quaternion.identity);
            Trails.Add(trail);
            indexList.Add(0);
        }
    }

    void FixedUpdate()
    {
        // ��¼��ͷ����ʷλ��
        if (posList.Count < MAX) posList.Add(player.transform.position);
        else posList[index] = player.transform.position;
        index = (index + 1) % MAX;

        // β�͸���
        for (int i = 0; i < Trails.Count; i++)
        {
            GameObject trail = Trails[i];
            if (i == 0)
            {
                // ��һ����β
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
