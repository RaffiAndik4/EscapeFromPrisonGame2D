using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceKillAndRestart : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string startSceneName = "Home";
    [SerializeField] private float restartDelay = 0.05f;

    [Header("Detection")]
    [SerializeField] private bool useTrigger = true; // true = Is Trigger ON

    [Header("SFX")]
    [SerializeField] private AudioClip caughtSfx;
    [Range(0f, 1f)] [SerializeField] private float caughtVolume = 1f;

    private bool triggered;

    private void KillPlayer(GameObject playerObj)
    {
        if (triggered) return;
        triggered = true;

        // 🔊 SFX ketangkep polisi
        if (caughtSfx != null && SFXManager.Instance != null)
            SFXManager.Instance.Play(caughtSfx, caughtVolume);

        // Matikan player agar terasa "mati"
        playerObj.SetActive(false);

        Invoke(nameof(Restart), restartDelay);
    }

    private void Restart()
    {
        SceneManager.LoadScene(startSceneName);
    }

    // Jika Collider pakai Is Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger) return;
        if (!other.CompareTag("Player")) return;

        KillPlayer(other.gameObject);
    }

    // Jika Collider pakai Collision biasa (Is Trigger OFF)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (useTrigger) return;
        if (!collision.collider.CompareTag("Player")) return;

        KillPlayer(collision.collider.gameObject);
    }
}
