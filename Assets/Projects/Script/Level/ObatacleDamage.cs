using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    public int damage = 1;

    // kunci supaya 1 obstacle hanya damage 1x per masuk
    private bool hasDamaged = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDamaged) return;

        PlayerHealth hp = other.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hasDamaged = true;
            hp.TakeDamage(damage);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // reset kunci saat player keluar spike
        if (other.GetComponent<PlayerHealth>() != null)
        {
            hasDamaged = false;
        }
    }
}
