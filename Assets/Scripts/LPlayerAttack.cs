using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPlayerAttack : MonoBehaviour
{

    public GameObject bolt;
    public Transform pos;
    SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl) && spriteRenderer.flipX == true){
            Instantiate(bolt, pos.position, Quaternion.Euler(0,180,0) );
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl) && spriteRenderer.flipX == false){
            Instantiate(bolt, pos.position, Quaternion.Euler(0,0,0) );
        }
    }
}
