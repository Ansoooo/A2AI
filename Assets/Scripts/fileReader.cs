using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class fileReader : MonoBehaviour
{
    //Singleton
    private static fileReader _instance;
    public static fileReader instance
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

    //File Vars
    public string fileName;
    public int doorNumber;
    private bool editedString = false;
    
    //Storage Vars
    public bool[] H, N, S;
    public float[] num;
    public int index;

    //Functionality
    void Start()
    {
        index = 0;
    }

    public void recieveInputFN(string name)
    {
        fileName = name;
        editedString = true;
    }
    public void recieveInputN(string _num)
    {
        doorNumber = int.Parse(_num);
    }
    public void readFile(string filePath)
    {
        if (editedString == true) //prevent reloading same file
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    line = sr.ReadLine();
                    index = 0;

                    while ((line = sr.ReadLine()) != null) // seperate text file parts into their values
                    {
                        string[] parts = line.Split('	');
                        storeValues(char.Parse(parts[0]), char.Parse(parts[1]), char.Parse(parts[2]), decimal.Parse(parts[3]), index);
                        index += 1;
                    }

                    spawnDoors.instance.destroyDoors(); // respawn appropriate number of doors
                    for (int i = 0; i < doorNumber; i++)
                    {
                        spawnDoors.instance.spawnDoor();
                    }

                    GameObject.Find("errorText").GetComponent<Text>().text = ""; // remove error
                }
            }
            catch (Exception e) // failed to open
            {
                GameObject.Find("errorText").GetComponent<Text>().text = e.Message; // send error
                Debug.Log(e.Message);
            }
            editedString = false;
        }
    }
    void storeValues(char _Hot, char _Noisy, char _Safe, decimal _number, int _index)
    {
        H[_index] = checkChar(_Hot); //store stuff into arrays
        N[_index] = checkChar(_Noisy);
        S[_index] = checkChar(_Safe);

        num[_index] = (float)_number;
    }
    bool checkChar(char _toCheck)
    {
        if (_toCheck == 'Y' || _toCheck == 'y') //check char value and turn it into bool
            return true;
        else
            return false;
    }
}
