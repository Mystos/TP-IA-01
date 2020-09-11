using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour
{
    public int dmg = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.isTrigger && collision.CompareTag("Enemy"))
        {
            float trans = GetComponentInParent<Transform>().position.x - collision.GetComponent<Transform>().position.x;
            bool recoilDirectionRight;
            if (trans > 0)
            {
                recoilDirectionRight = true;
            }
            else
            {
                recoilDirectionRight = false;
            }

            collision.SendMessageUpwards("RecoilDirection", recoilDirectionRight);
            collision.SendMessageUpwards("TakeDamage", dmg);
        }
    }
}
