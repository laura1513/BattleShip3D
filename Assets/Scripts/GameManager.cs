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
    //Boton para cambiar de barco
    private void NextShipClicked()
    {
        if (!shipScript.OnGameBoard())
        {
            shipScript.FlashColor(Color.yellow);
        }
        else
        {
            if(shipIndex <= ships.Length - 2)
            {
                shipIndex++;
                shipScript = ships[shipIndex].GetComponent<ShipScript>();
            } else
            {
                //Si ya se han colocado todos los barcos se desactivan el boton y el puerto
                nextBtn.gameObject.SetActive(false);
                puerto.SetActive(false);
                //Se cambia el texto de la parte superior
                topText.text = "Haz click en una casilla para lanzar un misil";
                //Se cambia el turno
                setupComplete = true;
                for (int i = 0; i < ships.Length; i++)
                {
                    ships[i].SetActive(false);
                }
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