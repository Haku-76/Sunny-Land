using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private Transform Left;
    private Transform Right;

    private float leftpoint;
    private float rightpoint;

    private bool faceleft = true;
    public float movespeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        // 获取Frog的Transform子类组件
        Left = this.transform.GetChild(0);
        Right = this.transform.GetChild(1);

        // 记录两个边界点的x坐标值
        leftpoint = Left.position.x;
        rightpoint = Right.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if(faceleft == true)
        {
            rb.velocity = new Vector2(-movespeed, rb.velocity.y);

            if(this.transform.position.x < leftpoint)
            {
                this.transform.localScale = new Vector3(-1, 1, 1);
                faceleft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(movespeed, rb.velocity.y);

            if (this.transform.position.x > rightpoint)
            {
                this.transform.localScale = new Vector3(1, 1, 1);
                faceleft = true;
            }
        }
    }

}
