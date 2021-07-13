using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{

    private int level = 1;

    // 分数
    private int score;
    // 拾取后蛇的速度倍数
    private float accelerate;
    // 增加蛇身节数
    private int length;
    // 颜色
    private Color color;
    // 材质号
    private int material;

    public int GetScore() { return score; }

    public float getAccelerate() { return accelerate; }

    public int getLength() { return length; }


    public void setLevel(int newLevel)
    {
        Debug.Log("set level: " + newLevel);
        switch(newLevel)
        {
            case 1:
                score = 10;
                accelerate = 1.5F;
                length = 1;
                material = 1;
                break;
            case 2:
                score = 20;
                accelerate = 2;
                length = 2;
                material = 2;
                break;
            case 3:
                score = 30;
                accelerate = 2;
                length = 3;
                material = 3;
                break;
        }
        level = newLevel;
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials\\Pickable" + material);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //GameControl.Pick(this);
            GameObject.Find("GameControl").GetComponent<GameControl>().Pick(this);
            Destroy(gameObject);
        }
    }
}
