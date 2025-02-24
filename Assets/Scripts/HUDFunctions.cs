using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDFunctions : MonoBehaviour
{
    public void CerrarAplicacion()
    {
        Application.Quit();
    }
    public void Replay()
    {
        SceneManager.LoadScene("Juego");
    }
}
