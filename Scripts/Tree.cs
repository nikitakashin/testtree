using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    Game g = new Game();   //Объект класса Game
    public GameObject[] treee = new GameObject[30]; //Префабы дерева
    public Vector3[] growPoints; //Масштабы при которых меняются префабы


    void Start()
    {


    }

    private Vector3 c = new Vector3(50, 50, 50);
    void Update()
    {
        grow();
        Debug.Log(growPoints);
    }

    public void grow()
    {
        int growPointsAmount = treee.Length; // Количество  этих масштабов сттрока 10

        growVectorValues(growPointsAmount);
    }

    public Vector3 growVectorValues(int i)
    {
        for (i = treee.Length; i < treee.Length; i++)  //treee.Length - Количество префабов (возможно надо сделать +1)
        {
            if (i <= 10)
            {
                int xyz = 1 + 10 * i;
                growPoints[i] = new Vector3(xyz, xyz, xyz);
            }
            else
            {
                int xyz = ((1 + 15 * i) * 2) - 7 * i;
                growPoints[i] = new Vector3(xyz, xyz, xyz);
            }
            return growPoints[i];
        }
        return growPoints[i];
    }
}
