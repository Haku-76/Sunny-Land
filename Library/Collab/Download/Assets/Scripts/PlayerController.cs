using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator anime;
    public float velocity;
    public float jumpforce;

    public float horizontalmove;
    public float facedirection;

    // Start is called before the first frame update
    void Start()
    {
        
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
        anime.SetFloat("Running",Mathf.Abs(facedirection));

        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2 (rb.velocity.y, jumpforce);
        }
    }

}
