using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Panggil ini dari tombol Start
    public void StartGame()
    {
        SceneManager.LoadScene("Level1_Penjara"); // ganti sesuai nama scene kamu
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
