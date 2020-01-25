using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnDoors : MonoBehaviour
{
    //Singleton
    private static spawnDoors _instance;
    public static spawnDoors instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    //Probability & Spawn Vars
    public float[] cumulativeSum;
    public GameObject prefab;
    private int totalSpawnedDoors = 0;
    public GameObject[] doorTracker;
    
    //Functionality
    private void getCSum() 
    {
        for(int i = 0; i < fileReader.instance.index; i++) // segment the total probability with individuals through cumulative sums
        {
            if (i == 0) // if first, then first num
            {
                cumulativeSum[i] = fileReader.instance.num[i]; 
            }
            else // else, then add num to previous sum
            {
                cumulativeSum[i] = fileReader.instance.num[i] + cumulativeSum[i - 1];
            }
        }
    }
    private float getRNum()
    {
        float num = Random.Range(0.0f, 1.0f); // roll number and return
        return(float)(num - (num % 0.01));
    }
    public int getDoorType()
    {
        float RNum = getRNum();
        getCSum();

        for(int i = 0; i < fileReader.instance.index; i++) // roll and check which door to spawn
        {
            if (i == 0) // if first, then
            { 
                if (RNum > 0 && RNum < cumulativeSum[0]) // if rolled the first 
                {
                    return 0; // return first door
                }
            }
            else // else, then
            {
                if (RNum > cumulativeSum[i - 1] && RNum < cumulativeSum[i]) // if rolled the first 
                {
                    return i; // return index door
                }
            }
        }

        return 0; // default :(
    }
    public void spawnDoor()
    {
        int type = getDoorType();

        GameObject doorType = Instantiate<GameObject>(prefab);
        doorType.SetActive(true);
        doorType.GetComponent<doors>().H = fileReader.instance.H[type];
        doorType.GetComponent<doors>().N = fileReader.instance.N[type];
        doorType.GetComponent<doors>().S = fileReader.instance.S[type];

        doorType.transform.position = new Vector3(totalSpawnedDoors * 5, doorType.transform.position.y, doorType.transform.position.z);
        doorTracker[totalSpawnedDoors] = doorType;
        totalSpawnedDoors += 1;
    }
    public void destroyDoors()
    {
       for(int i = 0; i < totalSpawnedDoors; i++)
       {
            Destroy(doorTracker[i]);
       }
       totalSpawnedDoors = 0;
    }
}
