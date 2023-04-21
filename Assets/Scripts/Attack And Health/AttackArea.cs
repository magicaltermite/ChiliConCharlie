using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] public int damage = 25;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var health = collision.GetComponent<Health>();
            health.Damage(damage);
        }
    }
}