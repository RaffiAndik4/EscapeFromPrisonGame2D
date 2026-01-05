using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHP = 3;

    [Tooltip("Cooldown setelah kena hit supaya tidak spam saat masih nempel obstacle.")]
    public float invincibleTime = 0.8f;

    [Header("Death / Respawn")]
    [Tooltip("Kalau true: reload scene aktif saat mati.")]
    public bool reloadCurrentSceneOnDeath = true;

    [Tooltip("Kalau reloadCurrentSceneOnDeath = false, akan load scene ini saat mati (contoh: Level1_Penjara).")]
    public string sceneOnDeath = "Level1_Penjara";

    [Tooltip("Delay sebelum pindah scene (biar anim Die sempat main).")]
    public float deathDelay = 0.8f;

    [Header("Animator (optional)")]
    public Animator animator;
    public string hurtTriggerName = "Hurt";
    public string dieTriggerName  = "Die";

    [Header("Feedback (optional)")]
    public SpriteRenderer spriteRenderer;
    public bool blinkWhenInvincible = true;
    public float blinkInterval = 0.1f;

    [Header("SFX (optional)")]
    public AudioClip hurtSfx;
    [Range(0f, 1f)] public float hurtVolume = 1f;

    public AudioClip dieSfx;
    [Range(0f, 1f)] public float dieVolume = 1f;

    private int hp;
    private bool invincible;
    private bool dead;

    public bool IsDead => dead;
    public bool IsInvincible => invincible;
    public int CurrentHP => hp;

    void Awake()
    {
        hp = maxHP;

        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int dmg)
    {
        if (dead || invincible) return;

        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            Die();
            return;
        }

        // SFX Hurt (hanya saat benar-benar kena damage & belum mati)
        if (hurtSfx != null && SFXManager.Instance != null)
            SFXManager.Instance.Play(hurtSfx, hurtVolume);

        // Play Hurt 1x
        TriggerIfExists(hurtTriggerName);

        StartCoroutine(InvincibleRoutine());
    }

    private void Die()
    {
        if (dead) return;
        dead = true;

        // SFX Die
        if (dieSfx != null && SFXManager.Instance != null)
            SFXManager.Instance.Play(dieSfx, dieVolume);

        TriggerIfExists(dieTriggerName);

        // Matikan movement biar tidak jalan lagi
        var move = GetComponent<PlayerMovement>();
        if (move != null) move.enabled = false;

        // Stop fisika
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Rigidbody2D pakai velocity
            rb.simulated = false;
        }

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator InvincibleRoutine()
    {
        invincible = true;

        if (!blinkWhenInvincible || spriteRenderer == null)
        {
            yield return new WaitForSeconds(invincibleTime);
            invincible = false;
            yield break;
        }

        float t = 0f;
        while (t < invincibleTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            t += blinkInterval;
        }

        spriteRenderer.enabled = true;
        invincible = false;
    }

    private IEnumerator DeathRoutine()
    {
        // WaitForSeconds berhenti kalau timeScale=0, jadi pakai Realtime biar aman.
        yield return new WaitForSecondsRealtime(deathDelay);

        if (reloadCurrentSceneOnDeath)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            if (!string.IsNullOrEmpty(sceneOnDeath))
                SceneManager.LoadScene(sceneOnDeath);
        }
    }

    private void TriggerIfExists(string trigger)
    {
        if (animator == null) return;
        if (string.IsNullOrEmpty(trigger)) return;

        foreach (var p in animator.parameters)
        {
            if (p.type == AnimatorControllerParameterType.Trigger && p.name == trigger)
            {
                animator.SetTrigger(trigger);
                return;
            }
        }
    }
}
