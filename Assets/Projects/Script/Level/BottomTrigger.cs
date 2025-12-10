using UnityEngine;

public class BottomTrigger : MonoBehaviour
{
    public FadeController fade;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fade.StartFadeOut(); // pindah ke Level 3
        }
    }
}
