using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
   
    public float xOffset = 0;
    public float zOffset = 0;
    private float nextYrotation = 90f;
    private GameObject clickedTile;
    int hitCount = 0;
    public int shipSize;

    private Material[] allMaterials;

    List<GameObject> touchTiles = new List<GameObject>();
    List<Color> allColors = new List<Color>();

    private void Start()
    {
        // Guardar los colores de los materiales del barco
        allMaterials = GetComponent<Renderer>().materials;
        for (int i = 0; i < allMaterials.Length; i++)
        {
            allColors.Add(allMaterials[i].color);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto colisionado es un tile
        if (collision.gameObject.CompareTag("Tile"))
        {
            touchTiles.Add(collision.gameObject);
        }
    }
    public void ClearTileList()
    {
        // Limpiar la lista de tiles
        touchTiles.Clear();
    }
    public Vector3 GetOffsetVec(Vector3 tilePos)
    {
        // Devolver un vector con la posición del tile
        return new Vector3(tilePos.x + xOffset, 2, tilePos.z + zOffset);
    }
    //Rotación del barco
    public void RotateShip()
    {
        touchTiles.Clear();
        transform.localEulerAngles += new Vector3(0, nextYrotation, 0);
        nextYrotation *= -1;
        float temp = xOffset;
        xOffset = zOffset;
        zOffset = temp;
        SetPosition(clickedTile.transform.position);
    }
    //Colocar el barco en la posición del tile
    public void SetPosition(Vector3 pos)
    {
        ClearTileList();
        transform.localPosition = new Vector3(pos.x + xOffset, 2, pos.z + zOffset);
    }
    //Añadir un tile a la lista de tiles
    public void SetClickedTile(GameObject tile)
    {
        clickedTile = tile;
    }

    public bool OnGameBoard()
    {
        // Verificar si el barco está en el tablero
        return touchTiles.Count == shipSize;
    }
    // Verificar si el barco está hundido
    public bool ComprobarHundido()
    {
        hitCount++;
        return shipSize <= hitCount;
    }

    //Cambiar el color del barco
    public void FlashColor(Color tempColor)
    {
        foreach (Material mat in allMaterials)
        {
            mat.color = tempColor;
        }
    }
    //Restablecer el color del barco
    private void ResetColor()
    {
        int i = 0;
        foreach (Material mat in allMaterials)
        {
            mat.color = allColors[i++];
        }
    }
}
