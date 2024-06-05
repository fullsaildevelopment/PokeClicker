using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void click()
    {
        float hp = GameManager.Instance.EnemyHP.fillAmount;
        hp -= 0.3f;
        if (hp <= 0)
            hp = 1;
        GameManager.Instance.EnemyHP.fillAmount = hp;
    }
}
