using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lbullet : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public GameObject spark;
    Vector3 pos;
    public float h;
    public float distance;
    public LayerMask isLayer;
    
    SpriteRenderer spriteRenderer;
    void Start()
    {   
        h = Input.GetAxisRaw("Horizontal");
        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.flipX = h == 1;
        Invoke("DestroyBullet",1.5f);
    }

    // Update is called once per frame
    void Update(){
        
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if(ray.collider != null){
            if(ray.collider.tag == "Enemy" || ray.collider.tag == "Platform"){
                DestroyBullet();
            }
        }
              
         if(transform.rotation.y == 0){
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else{
            transform.Translate(transform.right * speed * -1 * Time.deltaTime);
        }

    }
    void DestroyBullet(){
        Destroy(gameObject);
        pos = this.gameObject.transform.position;
        Instantiate(spark, pos , transform.rotation);
    }
}
