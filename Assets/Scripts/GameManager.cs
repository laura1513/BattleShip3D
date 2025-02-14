using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] ships;
    private bool setupComplete = false;
    private bool playerTurn = true;
    private int shipIndex = 0;
    private ShipScript shipScript;
    // Start is called before the first frame update
    void Start()
    {
        shipScript = ships[shipIndex].GetComponent<ShipScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TileClicked(GameObject tile)
    {
        if (setupComplete && playerTurn)
        {
            // se lanza un misil
        }
        else if (!setupComplete)
        {
            // se coloca un barco
            PlaceShip(tile);
        }
    }
    private void PlaceShip(GameObject tile)
    {
        // se coloca un barco en la casilla
        shipScript = ships[shipIndex].GetComponent<ShipScript>();
        shipScript.ClearTileList();
        Vector3 newVec = shipScript.GetOffsetVec(tile.transform.position);
        ships[shipIndex].transform.localPosition = newVec;
    }
}