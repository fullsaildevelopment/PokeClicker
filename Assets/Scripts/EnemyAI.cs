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
        if (startAttackAnim) //normal location: 0, 0 //peak attack = -40, -20
        {
            Vector3 location = GameManager.Instance.EnemySprite.transform.localPosition;
            if (!endAttackAnim)
            {
                location.x -= Time.deltaTime * 400;
                location.y -= Time.deltaTime * 200;
                if (location.x <= -40 && location.y <= -20)
                    endAttackAnim = true;
            }
            else
            {
                location.x += Time.deltaTime * 400;
                location.y += Time.deltaTime * 200;
                if (location.x >= 0 && location.y >= 0)
                {
                    startAttackAnim = false;
                    endAttackAnim = false;
                    location = Vector3.zero;
                }
            }

            GameManager.Instance.EnemySprite.transform.localPosition = location;
        }
    }
    public void Attack() //Called after a set time to attack the player. Damage math is done here.
    {
        if (Player.Instance.party.Count != 0)
        {
            if (Player.Instance.party[Player.Instance.currSlot].currHP > 0)
            {
                int damage = (int)(ClickerButtonScript.Instance.enemy.level * Random.Range(2, 5));
                float multiplier = GameManager.Instance.TypeMatchup(ClickerButtonScript.Instance.enemy, Player.Instance.party[Player.Instance.currSlot]);
                damage = (int)(damage * multiplier);
                damage /= 2; //Give the player an advantage (keep until multiple ways to heal are implemented)
                if (damage < 1)
                    damage = 1;
                startAttackAnim = true;
                Player.Instance.FlashPlayerEffectiveness(multiplier);
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
