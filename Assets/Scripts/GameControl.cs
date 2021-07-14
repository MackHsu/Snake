using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [Tooltip("�����ٶ�")]
    public float baseSpeed;
    [Tooltip("����ʱ��")]
    public float accelerateTime;
    [Tooltip("ʰȡ��Prefab")]
    public GameObject pickablePrefab;
    [Tooltip("ʰȡ��level1Ȩ��")]
    public float weight1;
    [Tooltip("ʰȡ��level2Ȩ��")]
    public float weight2;
    [Tooltip("ʰȡ��level3Ȩ��")]
    public float weight3;
    [Tooltip("�����")]
    public Text playerName;
    [Tooltip("����")]
    public Text score;
    [Tooltip("��Ϸ����ui")]
    public GameObject endUI;

    // ��ͷ��ʷλ���б�ѭ�����飩
    private static List<Vector3> posList;
    // posList����
    private static int MAX = 50000;
    // posList��ǰͷ���±�
    private static int index;

    // β���б�
    private static List<GameObject> Trails;
    // β��ǰ����λ����posList�е��±�
    private static List<int> indexList;

    // ��Ϸ������
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

        // ����β�ĳ�ʼλ����ӽ��б�
        posList.Add(player.transform.position - new Vector3(distance, 0, 0));
        index = 0;

        indexList = new List<int>();

        // ��ʼ��β��
        Vector3 formerPos = player.transform.position;
        for (int i = 0; i < startTrailNumber; i++)
        {
            GameObject trail = GameObject.Instantiate(trailPrefab, player.transform.position - new Vector3(distance, 0, 0), Quaternion.identity);
            Trails.Add(trail);
            indexList.Add(0);
            formerPos = trail.transform.position;
        }

        // ����ʰȡ��
        GeneratePickable();
    }

    void FixedUpdate()
    {
        if (!flag) return;

        // ��¼��ͷ����ʷλ��
        if (posList.Count < MAX) posList.Add(player.transform.position);
        else
        {
            posList[index] = player.transform.position;
            index = (index + 1) % MAX;
        }

        // β�͸���
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

    // ʰȡ
    public void Pick(Pickable p)
    {
        gm.CurrentRecord.score += p.GetScore();
        score.text = gm.CurrentRecord.score.ToString();
        AddLength(p.getLength());
        StopCoroutine("Accelerate"); // ���ǵ�ǰ�ļ���״̬
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
        // ����λ��
        Vector3 pos = new Vector3();
        while (true)
        {
            pos = new Vector3(Random.Range(-90, 90), 7.5F, Random.Range(-90, 90));
            if ((pos - player.transform.position).magnitude > distance) break;
        }

        GameObject pickable = GameObject.Instantiate(pickablePrefab, pos, Quaternion.identity);

        // ��������
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
