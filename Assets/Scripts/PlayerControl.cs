using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public FixedJoystick joystick;
    public float speed;
    private Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        dir = new Vector2(1, 0);
        this.gameObject.GetComponent<Rigidbody>().velocity = (new Vector3(dir.x, 0, dir.y)) * speed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newDir = joystick.Direction.normalized;
        if (newDir.magnitude == 0) return;
        else
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = (new Vector3(newDir.x, 0, newDir.y)) * speed;
        }
    }
}
