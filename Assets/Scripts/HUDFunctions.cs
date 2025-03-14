using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDFunctions : MonoBehaviour
{
    public float delay = 1f;

    public void CerrarAplicacion()
    {
        Application.Quit();
    }

    public void Replay()
    {
        StartCoroutine(CambiarEscenaConRetraso("Juego"));
    }

    public void Inicio()
    {
        StartCoroutine(CambiarEscenaConRetraso("Inicio"));
    }

    IEnumerator CambiarEscenaConRetraso(string sceneName)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
