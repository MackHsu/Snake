using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [Tooltip("�ƶ��ٶ�")]
    public float speed;

    [Tooltip("���ư�ť ��")]
    public Button up;
    [Tooltip("���ư�ť ��")]
    public Button down;
    [Tooltip("���ư�ť ��")]
    public Button left;
    [Tooltip("���ư�ť ��")]
    public Button right;

    // ��ǰ�ƶ��ķ���
    private Vector2 dir;

    void Start()
    {
        dir = new Vector2(1, 0);
        up.onClick.AddListener(() => Turn(new Vector2(0, 1)));
        down.onClick.AddListener(() => Turn(new Vector2(0, -1)));
        left.onClick.AddListener(() => Turn(new Vector2(-1, 0)));
        right.onClick.AddListener(() => Turn(new Vector2(1, 0)));
    }

    void FixedUpdate()
    {
        if (!GameControl.flag) return;

        Move(dir);
    }

    private void Turn(Vector2 newDir)
    {
        if (newDir.magnitude != 0 && Vector2.Angle(dir, newDir) != 180)
            dir = newDir;
    }

    void Move(Vector2 dir)
    {
        Vector2 newPos = new Vector2(transform.position.x + dir.x * speed * Time.deltaTime, transform.position.z + dir.y * speed * Time.deltaTime);
        transform.position = new Vector3(newPos.x, transform.position.y, newPos.y);
    }

}
