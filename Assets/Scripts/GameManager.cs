using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] ships;

    [Header("HUD")]
    public Button nextBtn;
    public Text topText;
    public Text playerShipText;
    public Text enemyShipText;

    [Header("Objects")]
    public GameObject missilePrefab;
    public GameObject enemyMissilePrefab;
    public GameObject puerto;
    public GameObject firePrefab;

    private bool setupComplete = false;
    private bool playerTurn = true;
    private int shipIndex = 0;
    private ShipScript shipScript;
    public EnemyIAScript enemyIAScript;

    private List<int[]> enemyShips;
    private List<GameObject> playerFires;

    private int enemyShipCount = 5;
    private int playerShipCount = 5;
    // Start is called before the first frame update
    void Start()
    {
        shipScript = ships[shipIndex].GetComponent<ShipScript>();
        nextBtn.onClick.AddListener(() => NextShipClicked());
        enemyShips = enemyIAScript.PlaceEnemyShips();
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

    public void CheckHit(GameObject tile)
    {
        int tileNum = Int32.Parse(Regex.Match(tile.name, @"\d+").Value);
        int hitCount = 0;
        foreach (int[] tileNumArray in enemyShips)
        {
            if (tileNumArray.Contains(tileNum))
            {
                for (int i = 0; i < tileNumArray.Length; i++)
                {
                    if (tileNumArray[i] == tileNum)
                    {

                        tileNumArray[i] = -5;
                        hitCount++;
                    }
                    else if (tileNumArray[i] == -5)
                    {
                        hitCount++;
                    }
                }
                if (hitCount == tileNumArray.Length)
                {
                    enemyShipCount--;
                    topText.text = "Hundido";

                }
                else
                {
                    topText.text = "Tocado";
                }
                break;
            }
            
        }
        if(hitCount == 0)
        {
            topText.text = "Agua";
        }
    }

    public void EnemyHitPlayer(Vector3 tile, int tileNum, GameObject hitObj)
    {
        enemyIAScript.missileHit(tileNum);
        tile.y += 0.2f;
        playerFires.Add(Instantiate(firePrefab, tile, Quaternion.identity));
        if (hitObj.GetComponent<ShipScript>().ComprobarHundido())
        {
            playerShipCount--;
            playerShipText.text = playerShipCount.ToString();
            enemyIAScript.HundirJugador();
        }
    }
}