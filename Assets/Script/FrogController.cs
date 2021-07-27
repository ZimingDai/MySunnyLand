using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    public LayerMask ground;
    public Transform leftPoint;
    public Transform rightPoint;

    private bool isFaceLeft = true;

    public float speed, jumpForce;

    private float leftx, rightx;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        //断绝子关系
        // transform.DetachChildren();
        leftx = leftPoint.position.x;
        rightx = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
       SwitchAnim();
    }

    void Movement()
    {
        // judge direction
        if (transform.position.x < leftx)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFaceLeft = false;
        }
        else if (transform.position.x > rightx)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFaceLeft = true;
        }
        
        // jump
        if (isFaceLeft)
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("isJumping", true);
                rb.velocity = new Vector2(-speed, jumpForce);
            }
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("isJumping", true);
                rb.velocity = new Vector2(speed, jumpForce);
            }
           
        }
    }

    void SwitchAnim()
    {
        if (anim.GetBool("isJumping"))
        {
            if (rb.velocity.y < 0.1f)
            {
                anim.SetBool("isFalling", true);
                anim.SetBool("isJumping", false);
            }
        }

        if (coll.IsTouchingLayers(ground) && anim.GetBool("isFalling"))
        {
            anim.SetBool("isFalling", false);
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        anim.SetTrigger("death");
    }
}
