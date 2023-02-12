using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public Collider2D DisColl;
    public Transform CeilingCheck, GroundCheck;
    //public AudioSource jumpAudio, hurtAudio, cherryAudio;

    public float speed;
    public float jumpforce;
    public Joystick joystick;
    public LayerMask ground;

    public int Cherry;
    public Text CherryNum;
    public int Gem;
    public Text GemNum;

    private bool isHurt;
    private bool isGround, isJump;
    bool jumpPressed;
    int jumpCount;

    void Start()
    {
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
        isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, ground);
        Jump();
    }

    private void Update()
    {
        //Jump();
        if(Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        Crouch();
        CherryNum.text = Cherry.ToString();
    }

    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");

        // 角色移动
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(facedirection));
        }

        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

    }

    void SwitchAnim()
    {
        //anim.SetBool("idle", false);
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                //anim.SetBool("idle", true);
                isHurt = false;
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            //anim.SetBool("idle", true);
        }
    }

    // 收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            collision.tag = "null";
            //cherryAudio.Play();
            //Destroy(collision.gameObject);
            //Cherry += 1;
            SoundManager.instance.CollectAudio();
            collision.GetComponent<Animator>().Play("isGot");

        }
        else if (collision.tag == "Collection2")
        {
            //cherryAudio.Play();
            SoundManager.instance.CollectAudio();
            Destroy(collision.gameObject);
            Gem += 1;
            GemNum.text = Gem.ToString();
        }

        if (collision.tag == "DeadLine")
        {
            //GetComponent<AudioSource>().enabled = false;
            SoundManager.instance.ShutDownBgm();
            Invoke("Restart", 2f);
        }
    }

    // 消灭敌人
    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling") && transform.position.y > (collision.gameObject.transform.position.y + 1))
            {
                enemy.Jumpon();
                rb.velocity = new Vector2(rb.velocity.x, 9);
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-9, rb.velocity.y);
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(9, rb.velocity.y);
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }
        }
    }

    //void Jump()
    //{
    //    // 角色跳跃
    //    if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
    //    {
    //        rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);
    //        jumpAudio.Play();
    //        anim.SetBool("jumping", true);

    //    }
    //}

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 1;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            SoundManager.instance.JumpAudio();
            jumpCount --;
            jumpPressed = false;
            anim.SetBool("jumping", true);
        }
        else if (jumpPressed && jumpCount > 0 && !isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            SoundManager.instance.JumpAudio();
            jumpCount--;
            jumpPressed = false;
            anim.SetBool("jumping", true);
        }
    }

    void Crouch()
    {
        if (!Physics2D.OverlapCircle(CeilingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("crouching", true);
                DisColl.enabled = false;
            }
            else
            {
                anim.SetBool("crouching", false);
                DisColl.enabled = true;
            }
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CherryCount()
    {
        Cherry += 1;
    }
}




//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        anim = GetComponent<Animator>();
//    }

//    void FixedUpdate()
//    {
//        if (!isHurt)
//        {
//            Movement();
//        }
//        SwitchAnim();
//        isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, ground);
//    }

//    private void Update()
//    {
//        //Jump();
//        Crouch();
//        CherryNum.text = Cherry.ToString();
//    }

//    void Movement()
//    {
//        float horizontalMove = joystick.Horizontal;
//        float facedirection = joystick.Horizontal;

//        // 角色移动
//        if (horizontalMove != 0f)
//        {
//            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
//            anim.SetFloat("running", Mathf.Abs(facedirection));
//        }

//        if (facedirection > 0f)
//        {
//            transform.localScale = new Vector3(1, 1, 1);
//        }

//        if (facedirection < 0f)
//        {
//            transform.localScale = new Vector3(-1, 1, 1);
//        }

//        if (horizontalMove == 0)
//        {
//            anim.SetFloat("running", 0);
//        }
//    }

//    void SwitchAnim()
//    {
//        //anim.SetBool("idle", false);
//        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
//        {
//            anim.SetBool("falling", true);
//        }
//        if (anim.GetBool("jumping"))
//        {
//            if (rb.velocity.y < 0)
//            {
//                anim.SetBool("jumping", false);
//                anim.SetBool("falling", true);
//            }
//        }
//        else if (isHurt)
//        {
//            anim.SetBool("hurt", true);
//            anim.SetFloat("running", 0);
//            if (Mathf.Abs(rb.velocity.x) < 0.1f)
//            {
//                anim.SetBool("hurt", false);
//                //anim.SetBool("idle", true);
//                isHurt = false;
//            }
//        }
//        else if (coll.IsTouchingLayers(ground))
//        {
//            anim.SetBool("falling", false);
//            //anim.SetBool("idle", true);
//        }
//    }

//    // 收集物品
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.tag == "Collection")
//        {
//            collision.tag = "null";
//            cherryAudio.Play();
//            //Destroy(collision.gameObject);
//            //Cherry += 1;
//            collision.GetComponent<Animator>().Play("isGot");

//        }
//        else if (collision.tag == "Collection2")
//        {
//            cherryAudio.Play();
//            Destroy(collision.gameObject);
//            Gem += 1;
//            GemNum.text = Gem.ToString();
//        }

//        if (collision.tag == "DeadLine")
//        {
//            GetComponent<AudioSource>().enabled = false;
//            Invoke("Restart", 2f);
//        }
//    }

//    // 消灭敌人
//    public void OnCollisionEnter2D(Collision2D collision)
//    {

//        if (collision.gameObject.tag == "Enemy")
//        {
//            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
//            if (anim.GetBool("falling"))
//            {
//                enemy.Jumpon();
//                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
//                anim.SetBool("jumping", true);
//            }
//            else if (transform.position.x < collision.gameObject.transform.position.x)
//            {
//                rb.velocity = new Vector2(-10, rb.velocity.y);
//                hurtAudio.Play();
//                isHurt = true;
//            }
//            else if (transform.position.x > collision.gameObject.transform.position.x)
//            {
//                rb.velocity = new Vector2(10, rb.velocity.y);
//                hurtAudio.Play();
//                isHurt = true;
//            }
//        }
//    }

//    //void Jump()
//    //{
//    //    // 角色跳跃
//    //    if (joystick.Vertical > 0.5f && coll.IsTouchingLayers(ground))
//    //    {
//    //        rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);
//    //        jumpAudio.Play();
//    //        anim.SetBool("jumping", true);

//    //    }
//    //}

//    void newJump()
//    {
//        if (isGround)
//        {
//            extraJump = 2;
//        }
//        if (inpi)
//    }

//    void Crouch()
//    {
//        if (!Physics2D.OverlapCircle(CeilingCheck.position, 0.2f, ground))
//        {
//            if (joystick.Vertical < -0.5f)
//            {
//                anim.SetBool("crouching", true);
//                DisColl.enabled = false;
//            }
//            else
//            {
//                anim.SetBool("crouching", false);
//                DisColl.enabled = true;
//            }
//        }
//    }

//    void Restart()
//    {
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }

//    public void CherryCount()
//    {
//        Cherry += 1;
//    }
//}
