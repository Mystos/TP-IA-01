using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAutoDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay = 0f;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }
}
