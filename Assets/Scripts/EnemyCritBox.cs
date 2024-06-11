using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCritBox : MonoBehaviour
{
    float deltaTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime >= 3)
        {
            Destroy(gameObject);
        }
    }
}
