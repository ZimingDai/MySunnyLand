using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    
    public LayerMask ground;
    public Collider2D topColl;
    public Collider2D coll;
    public Transform cellingCheck;
    public float speed = 10f;
    public float jumpForce;
    public int cherryNum = 0;
    public int gemNum = 0;

    public Text CherryNum;
    public Text GemNum;

    private bool isHurt;// default is false
    
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
            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
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
        Crouch();
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

    // 碰撞触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 收集樱桃
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

        if (collision.CompareTag("DeadLine"))
        {
            GetComponent<AudioSource>().enabled = false;
           Invoke("Restart", 1f); 
        }
    }
    
    // destroy enemies
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("isFalling"))
            {
                enemy.JumpOn();
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

    private void Crouch()
    // TODO:有问题！逻辑怪怪的
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position, 0.3f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("isCrouch", true);
                // 被启用
                topColl.enabled = false;
            } else
            {
                anim.SetBool("isCrouch", false);
                topColl.enabled = true;
            }
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
