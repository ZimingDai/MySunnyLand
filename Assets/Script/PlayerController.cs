using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    
    public LayerMask ground;
    public Collider2D coll;
    public float speed = 10f;
    public float jumpForce;
    public int cherryNum = 0;
    public int gemNum = 0;

    public Text CherryNum;
    public Text GemNum;

    private bool isHurt;// default is false
    
    // Start is called before the first frame update
    void Start()
    {
        //the game begin, import the things to rb,anim
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

     
    void FixedUpdate()
    {
        if (!isHurt)
        { 
            Movement();
        }
        SwitchAnim();
    }

    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");// 直接获得-1 - 1
        float faceDirection = Input.GetAxisRaw("Horizontal");//直接获得-1，0，1
        
        //Player move
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, rb.velocity.y);
            anim.SetFloat("running", math.abs(faceDirection));
        }
        if (faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }
        
        //Player jump
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            anim.SetBool("isJumping", true);
           
        }
    }
    void SwitchAnim()
    {
        anim.SetBool("isIdle", false);

        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("isFalling", true);
        }
        if (anim.GetBool("isJumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("isFalling", true);
                anim.SetBool("isJumping", false);
            }
        }else if (isHurt)
        {
            anim.SetBool("isHurt", true);
            anim.SetFloat("running", 0);
            if (math.abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("isHurt", false);
                anim.SetBool("isIdle", true);
                isHurt = false;
            }   
        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("isFalling", false);
            anim.SetBool("isIdle", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)// 这个参数
    {
        if (collision.CompareTag("Collection"))
        {
            Destroy(collision.gameObject);//销毁游戏体
            cherryNum += 1;
            CherryNum.text = cherryNum.ToString();
        }
        else if (collision.CompareTag("Gem_collection"))
        {
            Destroy(collision.gameObject);
            gemNum += 1;
            GemNum.text = gemNum.ToString();
        }
    }
    
    // destroy enemies
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            FrogController frog = collision.gameObject.GetComponent<FrogController>();
            if (anim.GetBool("isFalling"))
            {
                frog.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
                anim.SetBool("isJumping", true);
            } else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-8, rb.velocity.y);
                isHurt = true;
            } else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(8, rb.velocity.y);
                isHurt = true;
            }
        }
    }
}
