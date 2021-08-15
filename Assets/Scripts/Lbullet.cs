using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lbullet : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public GameObject spark;
    Vector3 pos;
    void Start()
    {
        Invoke("DestroyBullet",1.5f);
    }

    // Update is called once per frame
    void Update(){

        transform.Translate(transform.right * speed * Time.deltaTime);
        
    }
    void DestroyBullet(){
        Destroy(gameObject);
        pos = this.gameObject.transform.position;
        Instantiate(spark, pos , transform.rotation);
    }
}
