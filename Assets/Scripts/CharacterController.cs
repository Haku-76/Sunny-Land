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
        // �ٶȵ�����ֵ����ָ��ƶ�
        else if(anim.GetBool("IsHurting") == true && Mathf.Abs(rb.velocity.x) < 0.1f)
        {
            anim.SetBool("IsHurting", false);
        }

        UI();
    }

    void Movement()
    {
        // ��ȡ�ƶ���
        horizontalmove = Input.GetAxis("Horizontal");
        facedirection = Input.GetAxisRaw("Horizontal");

        // �����ٶ�
        rb.velocity = new Vector2(horizontalmove * velocity, rb.velocity.y);

        // ѡ��Ŀ�곯��
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

        // �����Ծ����������������Ծֵ
        if (Input.GetButtonDown("Jump") && jump_counter<jump_max)
        {
            jump_counter += 1;
            rb.velocity = new Vector2(rb.velocity.y, jumpforce);
            anim.SetBool("IsJumping", true);
        }

        // ����Ӵ���������Ծֵ���������Ծֵ
        if (coll.IsTouchingLayers(ground) && jump_counter == jump_max)
        {
            jump_counter = 0;
        }

        // ˮƽ�ʹ�ֱ���ٶ�Ϊ0ʱIsIdleingΪtrue
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            anim.SetBool("IsIdleing", true);
        }
        else
        {
            anim.SetBool("IsIdleing", false);
        }

        // Ŀ�겻��ʱ��IsRunningΪfalse
        if (facedirection == 0)
        {
            anim.SetBool("IsRunning", false);
        }
        else
        {
            anim.SetBool("IsRunning", true);   
        }

        // IsJumpingΪtrue�Ҵ�ֱ�ٶ�С��0ʱ,IsFallingΪtrue
        if (anim.GetBool("IsJumping") == true && rb.velocity.y < 0)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", true);
        }

        // ����Ӵ�����ʱ
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
