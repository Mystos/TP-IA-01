using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public bool facingRight = true;


    public GameObject target;
    public int moveSpeed;
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject redEyes;
    private Vector3 velocity;

    private void Update()
    {


        Flip(rb.velocity.x);

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        anim.SetFloat("xSpeed", characterVelocity);
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            EnemyMovement();
        }
    }

    void EnemyMovement()
    {
        Vector3 dir = target.transform.position - transform.position;
        //rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
        rb.AddForce(dir * moveSpeed);
    }

    void TargetChange(GameObject newTarget)
    {
        if(newTarget == null)
        {
            target = null;

        }
        if (newTarget != target)
        {
            target = newTarget;
            if (!facingRight)
            {
                Instantiate(redEyes, new Vector3(this.transform.position.x + -0.2f, this.transform.position.y + 0.1f), Quaternion.identity);
            }
            else
            {
                Instantiate(redEyes, new Vector3(this.transform.position.x + 0.2f, this.transform.position.y + 0.1f), Quaternion.identity);

            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.CompareTag("Player"))
        {
            TargetChange(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.CompareTag("Player"))
        {
            TargetChange(null);
        }
    }

    void Flip(float velocity)
    {

        Vector3 localScale = transform.localScale;
        if (velocity < -0.1)
        {
            facingRight = false;
        }
        else if (velocity > 0.1)
        {
            facingRight = true;
        }
        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;
    }
}
