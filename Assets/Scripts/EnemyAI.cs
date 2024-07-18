using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance;
    bool CanDealDamage;
    float deltaTime;
    bool startAttackAnim;
    bool endAttackAnim;
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
            if (deltaTime >= 1.5f)
            {
                deltaTime -= 1.5f;
                Attack();
            }
        }
        if (startAttackAnim) //normal location: 330, 0 //peak attack = 290, -20
        {
            Vector3 location = GameManager.Instance.EnemySprite.transform.localPosition;
            if (!endAttackAnim)
            {
                location.x -= Time.deltaTime * 400;
                location.y -= Time.deltaTime * 200;
                if (location.x <= 290 && location.y <= -20)
                    endAttackAnim = true;
            }
            else
            {
                location.x += Time.deltaTime * 400;
                location.y += Time.deltaTime * 200;
                if (location.x >= 330 && location.y >= 0)
                {
                    startAttackAnim = false;
                    endAttackAnim = false;
                    location.x = 330;
                    location.y = 0;
                }
            }

            GameManager.Instance.EnemySprite.transform.localPosition = location;
        }
    }
    public void Attack()
    {
        if (Player.Instance.party.Count != 0)
        {
            if (Player.Instance.party[Player.Instance.currSlot].currHP > 0)
            {
                int damage = (int)(ClickerButtonScript.Instance.enemy.level * Random.Range(2, 5));
                damage = (int)(damage * GameManager.Instance.TypeMatchup(ClickerButtonScript.Instance.enemy, Player.Instance.party[Player.Instance.currSlot]));
                damage /= 2;
                if (damage < 1)
                    damage = 1;
                startAttackAnim = true;
                Player.Instance.TakeDamage(damage);
            }
        }

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
