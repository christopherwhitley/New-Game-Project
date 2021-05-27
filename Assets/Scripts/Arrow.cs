using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField] GameObject arrow;
    GameObject projectile;
    SpriteRenderer sprite;
    public int damage = 10;
    EnemyBow enemy;
    

    private void Start()
    {
        enemy = GetComponentInParent<EnemyBow>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void CreateArrow()
    {

        GameObject projectile = Instantiate(arrow, transform.position, transform.rotation);

        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        

       

        if (enemy.facingRight == true)
        {
            projectileRigidbody.velocity = new Vector2(+10, projectileRigidbody.velocity.y);
            projectile.transform.localScale = new Vector3(-enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
        }
        else
        {

            projectileRigidbody.velocity = new Vector2(-10, projectileRigidbody.velocity.y);
            projectile.transform.localScale = new Vector3(enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
        }

        //projectile.transform.position = new Vector2(+2, transform.position.y);
        
    }

   

}
