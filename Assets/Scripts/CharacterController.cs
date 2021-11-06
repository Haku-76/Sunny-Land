using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    public Collider2D coll;
    public LayerMask ground;

    // Movement Variates
    public float velocity;
    public float jumpforce;

    public float horizontalmove;
    public float facedirection;

    // Collection Variates
    public int cherry_counter = 0;
    public int gem_counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        horizontalmove = Input.GetAxis("Horizontal");
        facedirection = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(horizontalmove * velocity, rb.velocity.y);

        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            anim.SetBool("IsIdleing", true);
        }
        else
        {
            anim.SetBool("IsIdleing", false);
        }

        if (facedirection == 0)
        {
            anim.SetBool("IsRunning", false);
        }
        else
        {
            anim.SetBool("IsRunning", true);   
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2 (rb.velocity.y, jumpforce);
            anim.SetBool("IsJumping", true);
        }

        if (anim.GetBool("IsJumping") == true && rb.velocity.y < 0)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", true);
        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("IsFalling", false);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collections")
        {
            Destroy(collision.gameObject);
            cherry_counter += 1;
        }
    }

}
