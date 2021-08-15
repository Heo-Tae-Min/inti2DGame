using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    int nextMove;


    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        Invoke("Think", 4);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // Platform check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        RaycastHit2D rayHit2 = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("MonsterOnlyRoad"));
        if (rayHit.collider == null && rayHit2.collider == null)
        {
            Turn();
        }
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 4);
    }

    void Think()
    {
        // Set Next Active
        nextMove = Random.Range(-1, 2);
        float nextThinkTime = Random.Range(2f, 6f);

        // Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);

        // Flip sprite
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        // Recursive
        Invoke("Think", nextThinkTime);
    }

    public void OnDamaged()
    {
        // Die Effect
        anim.SetTrigger("doDie");

        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Sprite Flip
        spriteRenderer.flipY = true;

        // Collider Disable
        capsuleCollider.enabled = false;

        // Destroy
        Invoke("DeActive", 1);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
