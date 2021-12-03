using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    public Collider2D coll;
    public LayerMask ground;
    public Text CherryCounter;
    public Text GemCounter;

    // Movement Variates
    public float velocity;
    public float jumpforce;
    public int jump_max = 2;
    public int jump_counter = 0;

    // Check move and direction
    private float horizontalmove;
    private float facedirection;

    // Collection Variates
    public int cherry = 0;
    public int gem = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("IsHurting") == false)
        {
            Movement();
        }
        // 速度低于阈值，则恢复移动
        else if(anim.GetBool("IsHurting") == true && Mathf.Abs(rb.velocity.x) < 0.1f)
        {
            anim.SetBool("IsHurting", false);
        }

        UI();
    }

    void Movement()
    {
        // 获取移动量
        horizontalmove = Input.GetAxis("Horizontal");
        facedirection = Input.GetAxisRaw("Horizontal");

        // 赋予速度
        rb.velocity = new Vector2(horizontalmove * velocity, rb.velocity.y);

        // 选择目标朝向
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

        // 检测跳跃输入且需低于最大跳跃值
        if (Input.GetButtonDown("Jump") && jump_counter<jump_max)
        {
            jump_counter += 1;
            rb.velocity = new Vector2(rb.velocity.y, jumpforce);
            anim.SetBool("IsJumping", true);
        }

        // 对象接触地面且跳跃值等于最大跳跃值
        if (coll.IsTouchingLayers(ground) && jump_counter == jump_max)
        {
            jump_counter = 0;
        }

        // 水平和垂直的速度为0时IsIdleing为true
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            anim.SetBool("IsIdleing", true);
        }
        else
        {
            anim.SetBool("IsIdleing", false);
        }

        // 目标不动时则IsRunning为false
        if (facedirection == 0)
        {
            anim.SetBool("IsRunning", false);
        }
        else
        {
            anim.SetBool("IsRunning", true);   
        }

        // IsJumping为true且垂直速度小于0时,IsFalling为true
        if (anim.GetBool("IsJumping") == true && rb.velocity.y < 0)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", true);
        }

        // 对象接触地面时
        if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("IsFalling", false);
            anim.SetBool("IsIdleing", true);
        }

    }

    void UI()
    {
        if (cherry == 0)
        {
            CherryCounter.text = "Cherry: 0";
        }
        else
        {
            CherryCounter.text = "Cherry: " + cherry.ToString();
        }

        if (gem == 0)
        {
            GemCounter.text = "Gem: 0";
        }
        else
        {
            GemCounter.text = "Gem: " + gem.ToString();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Cherrys")
        {
            Destroy(collision.gameObject);
            cherry += 1;         
        }

        if (collision.gameObject.tag == "Gems")
        {
            Destroy(collision.gameObject);
            gem += 1;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (anim.GetBool("IsFalling") == true)
            {
                Destroy(collision.gameObject);
                rb.velocity = new Vector2(rb.velocity.y, jumpforce);
                anim.SetBool("IsJumping", true);
            }
            else if(this.transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                anim.SetBool("IsHurting", true);
            }
            else if (this.transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                anim.SetBool("IsHurting", true);
            }
        }
    }

}
