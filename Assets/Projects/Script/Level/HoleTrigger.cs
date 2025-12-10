using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    public FadeController fade;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fade.StartFadeOut();
        }
    }
}
