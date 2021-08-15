using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    public float maxSpeed;
    public float counterJumpPower;
    public float jumpPower;
    public float isRight;
    public Transform frontCheck;
    public GameObject pickUp;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;

    private void Start() {
        Debug.Log("start");
    }

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
    void Update()
    {
        //right = 1, left = -1 variable
        if(spriteRenderer.flipX == false)
            isRight = 1;
        else
            isRight = -1;
        //Jump
        if(Input.GetButtonDown("Jump") && !anim.GetBool("isJumping")){
            if (!anim.GetBool("isWallStick"))
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
            }
            else
                anim.SetBool("isJumping", true);
        }
        if(Input.GetButtonDown("Jump") && anim.GetBool("isWallStick")){
            rigid.AddForce(Vector2.left * isRight * counterJumpPower, ForceMode2D.Impulse);
            rigid.AddForce(Vector2.up* jumpPower, ForceMode2D.Impulse);
        }

        //Stop speed
        if(Input.GetButtonUp("Horizontal")) // up = 떼는거
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f , rigid.velocity.y);
        //directon sprite
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1; // 왼쪽일때 flip x

        //Walking animation
        if(Mathf.Abs( rigid.velocity.x ) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        //wall sticking scan code
        if(true){
            Debug.DrawRay(rigid.position, Vector3.right * isRight , new Color(0,1,0));
            RaycastHit2D rayHitright = Physics2D.Raycast(rigid.position, Vector3.right * isRight, 1, LayerMask.GetMask("Platform"));
            //RaycastHit2D rayHitleft = Physics2D.Raycast(rigid.position, Vector3.left, 1, LayerMask.GetMask("Platform"));
            if(rayHitright.collider != null && rayHitright.distance < 0.5f ){
                Debug.Log("wall sticking");
                anim.SetBool("isWallStick", true);
            }
            else{
                anim.SetBool("isWallStick",false);
            }
        }


        //slowing at sticking wall
        if(anim.GetBool("isWallStick")){
            rigid.gravityScale = 0.5f;
        }
        else{
            rigid.gravityScale = 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        // move by key control
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse); // accelerator
        //rigid.velocity = new Vector2(maxSpeed*h, rigid.velocity.y); // fixed velocity

        //set Max speed
        if(rigid.velocity.x > maxSpeed) // right max speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // left max speed
            rigid.velocity = new Vector2(maxSpeed * (-1) ,rigid.velocity.y);

        //landing scanning code
        if(anim.GetBool("isJumping")){
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));
            RaycastHit2D rayHitDown = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rayHitDown.collider != null){
                if(rayHitDown.distance < 0.5f || rigid.velocity.y < 0.001){

                    anim.SetBool("isJumping", false);
                    Debug.Log("grounded");
                }

            }
            //Debug.Log(rayHit.collider.name);
        }



    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            // Damaged
            OnDamaged(collision.transform.position);
        }
        else if (collision.gameObject.tag == "Monster")
        {
            // Jump Attack
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else // Damaged
                OnDamaged(collision.transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            // Point
            bool isNormal = collision.gameObject.name.Contains("Normal");
            bool isPlus = collision.gameObject.name.Contains("Plus");
            bool isUltra = collision.gameObject.name.Contains("Ultra");

            if (isNormal)
                gameManager.stagePoint += 50;
            else if (isPlus)
                gameManager.stagePoint += 150;
            else if (isUltra)
                gameManager.stagePoint += 500;

            // Animation
            Instantiate(pickUp, transform.position, Quaternion.identity);

            // Deactive Item
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Finish")
        {
            //next Stage
        }
    }

    void OnAttack(Transform enemy)
    {
        // Point
        gameManager.stagePoint += 100;

        // Reaction Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Enemy Die
        HEnemyMove enemyMove = enemy.GetComponent<HEnemyMove>();
        enemyMove.OnDamaged();
    }

    void OnDamaged(Vector2 targetPos)
    {
        // Health Down
        gameManager.HealthDown();

        // Change Layer (Immortal Active)
        gameObject.layer = 9;

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.7f);

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? -1 : 1;
        rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);

        // Animation
        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 2);
    }

    void OffDamaged()
    {
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Sprite Flip Y
        spriteRenderer.flipY = true;

        // Collider Disable
        capsuleCollider.enabled = false;

        // Die Effect Jump
        rigid.AddForce(Vector2.up, ForceMode2D.Impulse);

        // Destroy
        Invoke("DeActive", 5);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
