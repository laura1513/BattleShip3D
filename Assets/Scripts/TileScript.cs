using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    GameManager gameManager;
    Ray ray;
    RaycastHit hit;
    private bool missileHit = false;
    // En C#, Color32[] representa un arreglo de estructuras Color32,
    // que es una estructura utilizada en Unity para representar colores con
    // componentes RGBA de 8 bits (valores entre 0 y 255).
    Color32[] hitColor = new Color32[2];

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0) && hit.collider.gameObject.name == gameObject.name)
            {
                if (missileHit == false)
                {
                    gameManager.TileClicked(hit.collider.gameObject);
                }
            }
        }
    }
}
