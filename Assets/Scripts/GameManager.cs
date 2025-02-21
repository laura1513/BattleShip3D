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
    public Text barcoOut;
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
        barcoOut.gameObject.SetActive(false);
    }
    //Boton para cambiar de barco
    private void NextShipClicked()
    {
        if (!shipScript.OnGameBoard())
        {
            barcoOut.gameObject.SetActive(true);
        }
        else
        {
            if (shipIndex <= ships.Length - 2)
            {
                shipIndex++;
                Debug.Log("Ship Index " + shipIndex);
                Debug.Log("Ship Length " + (ships.Length - 2));
                shipScript = ships[shipIndex].GetComponent<ShipScript>();
                barcoOut.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Barcos colocados");
                nextBtn.gameObject.SetActive(false);
                puerto.SetActive(false);
                barcoOut.gameObject.SetActive(false);
                topText.text = "Lanza la bomba";
                setupComplete = true;
                for (int i = 0; i < ships.Length; i++) ships[i].SetActive(false);
            }
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

    //Funcion para cuando se hace click en una casilla
    public void TileClicked(GameObject tile)
    {
        if (setupComplete && playerTurn)
        {
            // se lanza un misil
            Vector3 tilePos = tile.transform.position;
            tilePos.y += 200;
            playerTurn = false;
            Instantiate(missilePrefab, tilePos, missilePrefab.transform.rotation);
        }
        else if (!setupComplete)
        {
            // se coloca un barco
            PlaceShip(tile);
            shipScript.SetClickedTile(tile);
        }
    }
    //Funcion para colocar un barco
    private void PlaceShip(GameObject tile)
    {
        // se coloca un barco en la casilla
        shipScript = ships[shipIndex].GetComponent<ShipScript>();
        shipScript.ClearTileList();
        Vector3 newVec = shipScript.GetOffsetVec(tile.transform.position);
        ships[shipIndex].transform.localPosition = newVec;
    }
    //Funcion para lanzar un misil
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
            Debug.Log("Agua");
            topText.text = "Agua";
        }
    }
    //Funcion para cuando un misil enemigo impacta en un barco del jugador
    public void EnemyHitPlayer(Vector3 tile, int tileNum, GameObject hitObj)
    {
        enemyIAScript.MissileHit(tileNum);
        tile.y += 0.2f;
        playerFires.Add(Instantiate(firePrefab, tile, Quaternion.identity));
        if (hitObj.GetComponent<ShipScript>().ComprobarHundido())
        {
            playerShipCount--;
            playerShipText.text = playerShipCount.ToString();
            enemyIAScript.JugadorHundido();
        }
    }
}