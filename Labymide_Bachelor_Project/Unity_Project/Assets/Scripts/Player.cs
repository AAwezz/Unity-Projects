using UnityEngine;
using System.Collections;

/*
 * This scripts allows the player to see their given path.
 */

public class Player : MonoBehaviour
{
    public static GameObject player;
    public static bool colorOn = false;

    void OnMouseDown()
    {
        if (MainScript.oldGameObjectName == this.gameObject.name)
        {
            MainScript.ClearFieldsColor();
            MainScript.oldGameObjectName = "";
            colorOn = false;
        }
        else
        {
            MainScript.ClearFieldsColor();
            MainScript.oldGameObjectName = this.gameObject.name;
            ColorFields(this.gameObject);
            player = this.gameObject;
            colorOn = true;
        }
    }

    /* 
     * This method makes a colorfull path from your startfield to where you can reach.
     */
    public static void ColorFields(GameObject startField)
    {
        if (startField != null)
        {
            for (int i = 0; i < MainScript.allHoldersArray.Length; i++)
            {
                if (MainScript.allHoldersArray[i].transform.position.x == startField.transform.position.x - 1 && MainScript.allHoldersArray[i].transform.position.z == startField.transform.position.z)
                {
                    if (!MainScript.wallOnHolder[i])
                    {
                        for (int j = 0; j < MainScript.allFieldsArray.Length; j++)
                        {
                            if (MainScript.allFieldsArray[j].transform.position.x == startField.transform.position.x - 2 && MainScript.allFieldsArray[j].transform.position.z == startField.transform.position.z && MainScript.allFieldsArray[j].GetComponent<Renderer>().material.color != startField.GetComponent<Renderer>().material.color)
                            {
                                MainScript.allFieldsArray[j].GetComponent<Renderer>().material.color = startField.GetComponent<Renderer>().material.color;
                                ColorFields(MainScript.allFieldsArray[j]);
                            }
                        }
                    }
                }
                else if (MainScript.allHoldersArray[i].transform.position.x == startField.transform.position.x + 1 && MainScript.allHoldersArray[i].transform.position.z == startField.transform.position.z)
                {
                    if (!MainScript.wallOnHolder[i])
                    {
                        for (int j = 0; j < MainScript.allFieldsArray.Length; j++)
                        {
                            if (MainScript.allFieldsArray[j].transform.position.x == startField.transform.position.x + 2 && MainScript.allFieldsArray[j].transform.position.z == startField.transform.position.z && MainScript.allFieldsArray[j].GetComponent<Renderer>().material.color != startField.GetComponent<Renderer>().material.color)
                            {
                                MainScript.allFieldsArray[j].GetComponent<Renderer>().material.color = startField.GetComponent<Renderer>().material.color;
                                ColorFields(MainScript.allFieldsArray[j]);
                            }
                        }
                    }
                }
                else if (MainScript.allHoldersArray[i].transform.position.x == startField.transform.position.x && MainScript.allHoldersArray[i].transform.position.z == startField.transform.position.z - 1)
                {
                    if (!MainScript.wallOnHolder[i])
                    {
                        for (int j = 0; j < MainScript.allFieldsArray.Length; j++)
                        {
                            if (MainScript.allFieldsArray[j].transform.position.x == startField.transform.position.x && MainScript.allFieldsArray[j].transform.position.z == startField.transform.position.z - 2 && MainScript.allFieldsArray[j].GetComponent<Renderer>().material.color != startField.GetComponent<Renderer>().material.color)
                            {
                                MainScript.allFieldsArray[j].GetComponent<Renderer>().material.color = startField.GetComponent<Renderer>().material.color;
                                ColorFields(MainScript.allFieldsArray[j]);
                            }
                        }
                    }
                }
                else if (MainScript.allHoldersArray[i].transform.position.x == startField.transform.position.x && MainScript.allHoldersArray[i].transform.position.z == startField.transform.position.z + 1)
                {
                    if (!MainScript.wallOnHolder[i])
                    {
                        for (int j = 0; j < MainScript.allFieldsArray.Length; j++)
                        {
                            if (MainScript.allFieldsArray[j].transform.position.x == startField.transform.position.x && MainScript.allFieldsArray[j].transform.position.z == startField.transform.position.z + 2 && MainScript.allFieldsArray[j].GetComponent<Renderer>().material.color != startField.GetComponent<Renderer>().material.color)
                            {
                                MainScript.allFieldsArray[j].GetComponent<Renderer>().material.color = startField.GetComponent<Renderer>().material.color;
                                ColorFields(MainScript.allFieldsArray[j]);
                            }
                        }
                    }
                }
            }
        }
    }
}
