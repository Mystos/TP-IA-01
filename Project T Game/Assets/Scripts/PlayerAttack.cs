using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool attacking = false;
    public BoxCollider2D attackTrigger;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        attackTrigger.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && !attacking && anim.GetBool("IsGrounded") && !anim.GetBool("HasJustPressedJump") && !anim.GetBool("IsInInteraction"))
        {
            attacking = true;
            attackTrigger.enabled = true;
        }

        anim.SetBool("Attacking", attacking);
        anim.SetBool("AttackingTrigger", attackTrigger.enabled);
    }

    public void AttackAnimationAlerts(string message)
    {
        if (message.Equals("PlayerAttackStart"))
        {
            //attackTrigger.enabled = true;
        }
        if (message.Equals("AttackTriggerEnd"))
        {
            attackTrigger.enabled = false;
        }
        if (message.Equals("PlayerAttackEnd"))
        {
            attacking = false;
        }

    }
}
