using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileScript : MonoBehaviour
{
    GameManager gameManager;
    EnemyIAScript enemyIAScript;
    public Vector3 targetTileLocation;
    private int targetTile = -1;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyIAScript = GameObject.Find("Enemy").GetComponent<EnemyIAScript>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            gameManager.EnemyHitPlayer(targetTileLocation, targetTile, collision.gameObject);
        }
        else
        {

        }
        Destroy(gameObject);
    }
    public void SetTarget(int target)
    {
        targetTile = target;
    }
}
