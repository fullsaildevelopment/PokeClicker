using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance;
    public bool CanDealDamage;
    float deltaTime;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        CanDealDamage = false;
        deltaTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanDealDamage)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime >= 3)
            {
                deltaTime = 0;
                Attack();
            }
        }
    }
    void Attack()
    {
        Player.Instance.TakeDamage(1);
    }
}
