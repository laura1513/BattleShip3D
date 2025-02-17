using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] ships;

    [Header("HUD")]
    public Button nextBtn;

    private bool setupComplete = false;
    private bool playerTurn = true;
    private int shipIndex = 0;
    private ShipScript shipScript;
    public EnemyIAScript enemyIAScript;
    // Start is called before the first frame update
    void Start()
    {
        shipScript = ships[shipIndex].GetComponent<ShipScript>();
        nextBtn.onClick.AddListener(() => NextShipClicked());
    }
    private void NextShipClicked()
    {
        if (shipIndex < ships.Length - 1)
        {
            shipIndex++;
            shipScript = ships[shipIndex].GetComponent<ShipScript>();
        }
        else
        {
            setupComplete = true;
            nextBtn.gameObject.SetActive(false);
            enemyIAScript.PlaceEnemyShips();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //rotate ship
        if (Input.GetKeyDown(KeyCode.R))
        {
            shipScript.RotateShip();
        }
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
            shipScript.SetClickedTile(tile);
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