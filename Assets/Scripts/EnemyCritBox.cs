using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCritBox : MonoBehaviour
{
    float x;
    float y;
    float deltaTime;
    // Start is called before the first frame update
    void Start()
    {
        x = Random.Range(150, 500);
        y = Random.Range(50, 150);
        transform.localPosition = new Vector3(x, y, 0);
        deltaTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        y += Time.deltaTime * 150;
        transform.localPosition = new Vector3(x, y, 0);
        deltaTime += Time.deltaTime;
        if (deltaTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
