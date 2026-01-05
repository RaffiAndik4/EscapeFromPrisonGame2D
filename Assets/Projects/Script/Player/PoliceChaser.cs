using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceChase : MonoBehaviour
{
    [Header("Chase")]
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float stopDistance = 0.2f;

    private Transform player;

    private Vector3 baseScale; // simpan scale awal

    private void Awake()
    {
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindPlayer();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        player = go != null ? go.transform : null;
    }

    private void Update()
    {
        if (player == null) return;

        Vector2 toPlayer = (Vector2)(player.position - transform.position);
        float dist = toPlayer.magnitude;
        if (dist <= stopDistance) return;

        Vector2 dir = toPlayer.normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);

        // Flip aman: tetap pakai scale awal (misal 1.4)
        if (Mathf.Abs(dir.x) > 0.001f)
        {
            float sign = dir.x >= 0f ? 1f : -1f;
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x) * sign, baseScale.y, baseScale.z);
        }
    }
}
