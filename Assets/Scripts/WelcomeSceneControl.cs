using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeSceneControl : MonoBehaviour
{
    [Tooltip("����������")]
    public InputField playerNameInput;
    [Tooltip("��ʼ��ť")]
    public Button startBtn;
    [Tooltip("���а�ť")]
    public Button scoreListBtn;
    [Tooltip("���а񴰿�")]
    public GameObject scoreListWindow;
    [Tooltip("���а��б�")]
    public HorizontalLayoutGroup scoreList;
    [Tooltip("���а����Prefab")]
    public GameObject scoreListObjectPrefeb;
    [Tooltip("���а�رհ�ť")]
    public Button scoreListCloseBtn;

    private string playerName;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetManager();
        playerNameInput.onValueChanged.AddListener(param => InputChangeValue(param));
        startBtn.onClick.AddListener(StartGame);
        scoreListBtn.onClick.AddListener(OpenScoreListWindow);
        scoreListCloseBtn.onClick.AddListener(() => scoreListWindow.SetActive(false));
    }

    private void InputChangeValue(string str)
    {
        if (str.Length > 0) startBtn.interactable = true;
        else startBtn.interactable = false;
        playerName = str;
    }

    private void StartGame()
    {
        GameManager gm = GameManager.GetManager();
        gm.CurrentRecord.playerName = playerName;
        gm.CurrentRecord.score = 0;
        SceneManager.LoadScene("PlayScene");
    }

    private void OpenScoreListWindow()
    {
        scoreListWindow.SetActive(true);
        
        // ����б�
        for (int i = 0; i < scoreList.transform.childCount; i++)
        {
            Destroy(scoreList.transform.GetChild(i).gameObject);
        }

        if (gm.Records.Count == 0) return;

        int rank = 1; // ����
        float temp = gm.Records[0].score; // ǰһ���ķ���
        for (int i = 0; i < gm.Records.Count; i++)
        {
            if (gm.Records[i].score < temp)
            {
                rank++;
            }
            if (rank > 10) break;

            GameObject listObject = GameObject.Instantiate(scoreListObjectPrefeb, scoreList.transform);
            Color color = Color.black;
            switch (rank)
            {
                case 1:
                    color = Color.red;
                    break;
                case 2:
                    color = Color.yellow;
                    break;
                case 3:
                    color = Color.blue;
                    break;
            }

            Text rankText = listObject.transform.Find("Rank").GetComponent<Text>();
            rankText.color = color;
            rankText.text = rank.ToString();

            Text name = listObject.transform.Find("Name").GetComponent<Text>();
            name.color = color;
            name.text = gm.Records[i].playerName;

            Text score = listObject.transform.Find("Score").GetComponent<Text>();
            score.color = color;
            score.text = gm.Records[i].score.ToString();
        }
    }
}
