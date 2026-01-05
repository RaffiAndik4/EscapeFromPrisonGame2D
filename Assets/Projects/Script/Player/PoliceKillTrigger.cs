using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceKillTrigger : MonoBehaviour
{
    [SerializeField] private string startSceneName = "Home";
    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        other.gameObject.SetActive(false);
        SceneManager.LoadScene(startSceneName);
    }
}
