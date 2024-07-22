using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI message;
    float x;
    float y;
    float deltaTime;
    // Start is called before the first frame update
    void Start()
    {
        int damage = 0;
        if (transform.parent == GameManager.Instance.EnemySprite.transform.parent)
            damage = ClickerButtonScript.Instance.takenDamage;
        else if (transform.parent == GameManager.Instance.PlayerSprite.transform.parent)
            damage = Player.Instance.damageTaken;
        x = Random.Range(-200, 200);
        y = Random.Range(100, 200);
        transform.localPosition = new Vector3(x, y, 0);
        deltaTime = 0;

        if (damage < 0)
        {
            message.color = Color.green;
            message.text = "+" + damage.ToString();
        }
        else if (damage > 0)
        {
            message.color = Color.red;
            message.text = "-" + damage.ToString();
        }
        else
            Destroy(gameObject); //if its 0 damage, no need to have any feedback
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
