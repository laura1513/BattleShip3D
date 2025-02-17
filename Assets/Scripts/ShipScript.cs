using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    List<GameObject> touchTiles = new List<GameObject>();
    public float xOffset = 0;
    public float zOffset = 0;
    private float nextYrotation = 90f;
    private GameObject clickedTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClearTileList()
    {
        touchTiles.Clear();
    }
    public Vector3 GetOffsetVec(Vector3 tilePos)
    {
        return new Vector3(tilePos.x + xOffset, 2, tilePos.z + zOffset);
    }

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
    public void SetPosition(Vector3 pos)
    {
        transform.localPosition = new Vector3(pos.x + xOffset, 2, pos.z + zOffset);
    }  

    public void SetClickedTile(GameObject tile)
    {
        clickedTile = tile;
    }
}
