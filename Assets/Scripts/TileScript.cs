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
        hitColor[0] = gameObject.GetComponent<MeshRenderer>().material.color;
        hitColor[1] = gameObject.GetComponent<MeshRenderer>().material.color;
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Missile")
        {
            missileHit = true;
        }
        else if (collision.gameObject.tag == "EnemyMissile")
        {
            hitColor[0] = new Color32(38, 57, 76, 2);
            GetComponent<Renderer>().material.color = hitColor[0];
        }
    }
    public void SetTileColor(int index, Color32 color)
    {
        hitColor[index] = color;
    }
    public void SwitchColors(int colorIndex)
    {
        GetComponent<Renderer>().material.color = hitColor[colorIndex];
    }
}
