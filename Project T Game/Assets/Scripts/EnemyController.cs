using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public float recoilForce;

    public int force;
    public int dex;

    private bool isDead = false;
    private bool takeHit = false;
    public Animator anim;
    public Rigidbody2D rb;
    public BoxCollider2D attackTrigger;
    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        attackTrigger.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<EnemyBehaviour>().target != null && GetComponent<EnemyBehaviour>().target.CompareTag("Player"))
        {
            if (Vector3.Distance(GetComponent<EnemyBehaviour>().target.transform.position, this.transform.position) < 1 && !isAttacking)
            {
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }
        }


        anim.SetBool("IsAttacking", isAttacking);
        anim.SetBool("AttackingTrigger", attackTrigger.enabled);
        anim.SetBool("IsDead", isDead);
        anim.SetBool("TakeHit", takeHit);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        takeHit = true;
        rb.velocity = new Vector2(0f, 0f);
        rb.AddForce(new Vector2(recoilForce , recoilForce));
    }

    void RecoilDirection(bool recoilDirectionRight)
    {
        if (recoilDirectionRight && recoilForce > 1)
        {
            recoilForce *= -1;
        }
        else if(!recoilDirectionRight && recoilForce <1)
        {
            recoilForce *= -1;
        }
    }

    public void AnimationEnemyEventController(string message)
    {
        if (message.Equals("IsDead"))
        {
            Destroy(gameObject);
        }
        if (message.Equals("TakeHitAnimationEnd"))
        {
            takeHit = false;
            if (currentHealth <= 0)
            {
                isDead = true;
            }
        }
    }
}
