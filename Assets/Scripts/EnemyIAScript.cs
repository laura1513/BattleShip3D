using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyIAScript : MonoBehaviour
{
    char[] guessGrid;
    List<int> potentialHits;
    List<int> currentHits;
    private void Start()
    {
        potentialHits = new List<int>();
        currentHits = new List<int>();
        guessGrid = Enumerable.Repeat('o', 100).ToArray();
    }
    public List<int[]> PlaceEnemyShips()
    {
        List<int[]> enemyShips = new List<int[]>
        {
            new int[] { -1, -1, -1, -1, -1 },
            new int[] { -1, -1, -1, -1 },
            new int[] { -1, -1, -1 },
            new int[] { -1, -1, -1 },
            new int[] { -1, -1, }
        };
        int[] gridNumbers = Enumerable.Range(0, 100).ToArray();
        bool taken = true;
        foreach (int[] tileNumArray in enemyShips)
        {
            taken = true;
            while (taken)
            {
                taken = false;
                int shipNose = UnityEngine.Random.Range(0, 99);
                int rotateBool = UnityEngine.Random.Range(0, 2);
                int minusAmount = rotateBool == 0 ? 10 : 1;
                for (int i = 0; i < tileNumArray.Length; i++)
                {
                    //check that ship end will not go off board and check if tile is taken
                    if ((shipNose - (minusAmount * i) < 0) || (gridNumbers[shipNose - i * minusAmount]) < 0)
                    {
                        taken = true;
                        break;
                    }
                    //Ship is horizontal, check ship doesnt go off the sides 0 to 10, 11 to 20
                    else if(minusAmount == 1 && shipNose / 10 != ((shipNose - i * minusAmount)-1) / 10)
                    {
                        taken = true;
                        break;
                    }
                }
                //if tile is not taken, loop through tile numbers assign them to the array in the list
                if (taken == false)
                {
                    for(int j = 0; j < tileNumArray.Length; j++)
                    {
                        tileNumArray[j] = gridNumbers[shipNose - j * minusAmount];
                        gridNumbers[shipNose - j * minusAmount] = -1;
                    }
                }
            }
        }

            return enemyShips;
    }

}
