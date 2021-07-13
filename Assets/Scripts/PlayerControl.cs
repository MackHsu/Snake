using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Tooltip("摇杆")]
    public FixedJoystick joystick;
    [Tooltip("移动速度")]
    public float speed;

    // 当前移动的方向
    private Vector2 dir;

    void Start()
    {
        dir = new Vector2(1, 0);
    }

    void FixedUpdate()
    {
        if (!GameControl.flag) return;
        Vector2 newDir = joystick.Direction;
        if (newDir.magnitude != 0 && Vector2.Angle(dir, newDir) != 180)
            dir = newDir;

        Move(dir);
    }

    void Move(Vector2 dir)
    {
        Vector2 newPos = new Vector2(transform.position.x + dir.x * speed * Time.deltaTime, transform.position.z + dir.y * speed * Time.deltaTime);
        transform.position = new Vector3(newPos.x, transform.position.y, newPos.y);
    }

}
