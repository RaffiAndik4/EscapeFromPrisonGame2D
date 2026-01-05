using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip mainMenuBgm;
    [SerializeField] private AudioClip tunnelBgm;
    [SerializeField] private AudioClip gameplayBgm;
    [SerializeField] private AudioClip finishBgm;

    [Header("Scene Names (harus sama persis)")]
    [SerializeField] private string mainMenuSceneName = "Home";
    [SerializeField] private string tunnelSceneName = "Tunnel";
    [SerializeField] private string gameplaySceneName = "Level1_Penjara";
    [SerializeField] private string finishSceneName = "Finish";

    [Header("Volume")]
    [Range(0f, 1f)] [SerializeField] private float volume = 0.35f;

    private AudioSource source;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        if (source == null) source = gameObject.AddComponent<AudioSource>();

        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.volume = volume;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        UpdateBGM(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateBGM(scene.name);
    }

    private void UpdateBGM(string sceneName)
    {
        AudioClip target = GetClipForScene(sceneName);
        if (target == null) return;

        // 🔑 Anti restart: kalau sama & lagi main, jangan Play ulang
        if (source.clip == target && source.isPlaying) return;

        source.clip = target;
        source.volume = volume;
        source.Play();
    }

    private AudioClip GetClipForScene(string sceneName)
    {
        if (sceneName == mainMenuSceneName) return mainMenuBgm;
        if (sceneName == tunnelSceneName) return tunnelBgm;
        if (sceneName == finishSceneName) return finishBgm;

        // Default: semua scene selain 3 di atas dianggap gameplay
        // (jadi kamu nggak perlu isi nama scene satu-satu)
        return gameplayBgm;
    }

    public void SetVolume(float v)
    {
        volume = Mathf.Clamp01(v);
        if (source != null) source.volume = volume;
    }
}
