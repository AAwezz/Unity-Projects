using UnityEngine;
using System.Collections;

/*
 * This is the script to roll the dice.
 */

public class Dice : MonoBehaviour
{
    public static int sideOnDice;
    public static bool dicedRolled = true, once = false;
    float time;
    Vector3 throwRollVec = new Vector3(0, 0, 0);
    Vector3 currentRotation = new Vector3(0, 90, 0);
    Vector3 one = new Vector3(180, 90, 0);
    Vector3 two = new Vector3(90, 90, 90);
    Vector3 three = new Vector3(180, 90, 90);
    Vector3 four = new Vector3(0, 90, 90);
    Vector3 five = new Vector3(270, 90, 90);
    Vector3 six = new Vector3(0, 90, 0);
    Vector3 endingRotation;

    /* 
     * In this update we first rotate the dice 360 degress and then lerp to the right position.
     */
    void Update()
    {
        if (once || transform.position != endingRotation)
        {
            LerpDice();
            ChangePosTo(currentRotation);
            once = false;
        }
    }

    void OnMouseDown()
    {
        if (dicedRolled && ServerHub.playerNames[MainScript.playerTurn] == NetworkManager.showName)
        {
            dicedRolled = false;
            sideOnDice = Random.Range(1, 7);
            GetComponent<NetworkView>().RPC("AfterRollSetup", RPCMode.AllBuffered, sideOnDice, true);
        }
    }

    private void ThrowRoll()
    {
        if (throwRollVec.Equals(new Vector3(0, 0, 0)))
        {
            throwRollVec = new Vector3(360, 360, 360);
        }
        else
        {
            throwRollVec = new Vector3(0, 0, 0);
        }
    }

    /* 
     * Setting the currentRotation for each frame, on where the dice need to rotate to.
     */
    void LerpDice()
    {
        switch (sideOnDice)
        {
            case 1:
                currentRotation = Vector3.Lerp(currentRotation, one + throwRollVec, time);
                endingRotation = one + throwRollVec;
                break;
            case 2:
                currentRotation = Vector3.Lerp(currentRotation, two + throwRollVec, time);
                endingRotation = two + throwRollVec;
                break;
            case 3:
                currentRotation = Vector3.Lerp(currentRotation, three + throwRollVec, time);
                endingRotation = three + throwRollVec;
                break;
            case 4:
                currentRotation = Vector3.Lerp(currentRotation, four + throwRollVec, time);
                endingRotation = four + throwRollVec;
                break;
            case 5:
                currentRotation = Vector3.Lerp(currentRotation, five + throwRollVec, time);
                endingRotation = five + throwRollVec;
                break;
            case 6:
                currentRotation = Vector3.Lerp(currentRotation, six + throwRollVec, time);
                endingRotation = six + throwRollVec;
                break;
        }
    }

    /* 
     * A RPC call to give the other clients the same values.
     */
    [RPC]
    void AfterRollSetup(int d,bool b)
    {
        once = b;
        sideOnDice = d;
        ThrowRoll();
        time = Time.deltaTime * 2;
        MainScript.ClearWallColor();
        MainScript.AvaliableWalls();
        Holders.wallMoved = false;
    }

    /* 
     * This changes the position of the dice to all the clients.
     */
    void ChangePosTo(Vector3 vec)
    {
        transform.eulerAngles = new Vector3(vec.x, vec.y, vec.z);
    }
}