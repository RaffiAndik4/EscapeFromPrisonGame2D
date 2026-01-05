using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string firstLevelSceneName = "Level1_Penjara";

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void ExitGame()
    {
        // Keluar saat sudah jadi build (EXE)
        Application.Quit();

        // Biar enak dites di Unity Editor (tidak ngaruh di EXE)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
