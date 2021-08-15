using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSpark : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestorySpark",0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DestorySpark(){
        Destroy(gameObject);
    }
}
