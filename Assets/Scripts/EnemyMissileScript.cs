using UnityEngine;

public class EnemyMissileScript : MonoBehaviour
{
    GameManager gameManager;
    EnemyIAScript enemyIAScript;
    public Vector3 targetTileLocation;
    private int targetTile = -1;

    //Añadir sonidos de agua o explosion al impactar
    public AudioSource audioSource;
    public AudioClip waterSound;
    public AudioClip explosionSound;

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
            gameManager.Sonidos(explosionSound);
            gameManager.EnemyHitPlayer(targetTileLocation, targetTile, collision.gameObject);
        }
        else
        {
            gameManager.Sonidos(waterSound);
            gameManager.EnemyMissed(targetTileLocation, targetTile);
            Debug.Log("Enemy missed");
            enemyIAScript.PauseAndEnd(targetTile);
        }
        Destroy(gameObject);
    }
    public void SetTarget(int target)
    {
        targetTile = target;
    }
}
