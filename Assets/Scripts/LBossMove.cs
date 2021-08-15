using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBossMove : MonoBehaviour
{
    public float bossHealth;
    // Start is called before the first frame update
    public Transform target;
    public float direction;
    public float acceleration;
    public float velocity;
    SpriteRenderer spriteRenderer; 
    Rigidbody2D rigid;
    Animation anim;
    public int nextMove;
    public int nextJump;
    public int jumpPower;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animation>();
        Invoke("bossAI",2);
    }

    // Update is called once per frame
    void Update()
    {
        //where is player?
        target = GameObject.Find("Player").transform;
        //target - boss direction check
        direction = Mathf.Abs(target.position.x - transform.position.x);
        if(rigid.velocity.x > 0){ // going left,have to flip
            spriteRenderer.flipX = true;
        }
        else
            spriteRenderer.flipX = false;
        
        

    }
    private void FixedUpdate() {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        
    }
    public void bossAI(){
        if (direction < 10) {
            nextMove = Random.Range(-3, 4);
            nextJump = Random.Range(0, 3);
            jumpPower = Random.Range(0, 6);
            if (nextJump == 2) {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }

            // rest for 5 second
            if (bossHealth == 50) {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                Invoke("bossAI", 5);
            }
            else
                Invoke("bossAI", 1);
        }

    }
    public void Turn(){
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("bossAI",0.5f);
    }
    
    
    
}
