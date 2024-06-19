using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance;
    bool CanDealDamage;
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
    public void Attack()
    {
        int damage = (int)(ClickerButtonScript.Instance.enemy.level * 1.5f);
        damage = (int)(damage * GameManager.Instance.TypeMatchup(ClickerButtonScript.Instance.enemy, Player.Instance.party[Player.Instance.currSlot])) / 3;
        Player.Instance.TakeDamage(damage);
    }
    public void ResetAttack()
    {
        deltaTime = 0;
    }
    public void PauseAttack(bool pause)
    {
        CanDealDamage = !pause;
        deltaTime = 0;
    }
}
