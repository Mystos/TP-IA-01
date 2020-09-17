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
    public bool useAEtoile = true;
    private Vector3 velocity;
    private List<Vector3> pathToTarget;

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
        Vector3 direction;
        pathToTarget = Pathfinder.Instance.FindPath(transform.position, target.transform.position, useAEtoile);
        if (pathToTarget != null)
        {
            for (int i = 0; i < pathToTarget.Count - 1; i++)
            {
                Debug.DrawLine(pathToTarget[i], pathToTarget[i + 1], Color.red);
            }
        }
        if(useAEtoile)
        {
            direction = (pathToTarget[1] - transform.position).normalized;
        }
        else
        {
            direction = (pathToTarget[0] - transform.position).normalized;
        }


        if (Vector3.Distance(transform.position, target.transform.position) > 1f)
        {
            //rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
            Vector3 vec = transform.position + direction * moveSpeed * Time.deltaTime;
            rb.MovePosition(vec);
        }



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
            //if (!facingRight)
            //{
            //    Instantiate(redEyes, new Vector3(this.transform.position.x + -0.2f, this.transform.position.y + 0.1f), Quaternion.identity);
            //}
            //else
            //{
            //    Instantiate(redEyes, new Vector3(this.transform.position.x + 0.2f, this.transform.position.y + 0.1f), Quaternion.identity);

            //}

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
