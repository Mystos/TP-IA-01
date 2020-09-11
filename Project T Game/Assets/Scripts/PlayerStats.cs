    using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public float recoilForce;

    public Rigidbody2D rb;
    public Animator anim;
    public int force;
    public int dex;

    private bool isDead = false;
    private bool takeHit = false;
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("IsDead", isDead);
        anim.SetBool("TakeHit", takeHit);
    }

    void RecoilDirection(bool recoilDirectionRight)
    {
        if (recoilDirectionRight && recoilForce > 1)
        {
            recoilForce *= -1;
        }
        else if (!recoilDirectionRight && recoilForce < 1)
        {
            recoilForce *= -1;
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        takeHit = true;
        rb.velocity = new Vector2(0f, 0f);
        rb.AddForce(new Vector2(recoilForce, recoilForce));
    }

    public void PlayerAnimEventStats(string message)
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
