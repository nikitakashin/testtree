using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;

public class Game : MonoBehaviour
{

    /*
    private void Awake()
    {
        
    }
    */

    [Header("Высота дерева")]
    [Tooltip("Его высота в метрах")]
    public float treeHeight;
    [Header("Скорость роста дерева")]
    [Tooltip("Скорость роста дерева в м/с")]
    public float growSpeed;
    [Space]
    [Header("Скорость роста реального дуба")]
    [Tooltip("метры/год")]
    public int oak = 2;
    [Header("Ускорение относительно реального мира")]
    [Tooltip("На это число мы умножаем скорость")]
    public int speeder = 16;

    //Коэффициенты, влияющие на скорость роста
    [Header("Коэффициент воды")]
    public float waterRatio;
    [Header("Коэффициент земли")]
    public float landRatio;
    [Header("Коэффициент солнца")]
    public float sunRatio;
    [Header("Коэффициент здоровья")]
    public float healthRatio;
    private float ratios; // Произведение всех коэффициентов

    //Объявляем переменные текста
    public Text treeHeightText;
    public Text growSpeedText;
    public Text waterRatioText;
    public Text landRatioText;
    public Text sunRatioText;
    public Text healthRatioText;
    public Text timeUtcText;
    public Text timePhoneText;

    void Start()
    {

        

        StartCoroutine(updateRatios());

        var dateTime = CheckGlobalTime(); //Получаеем время UTC
        var actualTime = System.DateTime.Now.ToString("hh:mm:ss");
        var date = System.DateTime.Now.ToString("dd/MM/yyyy");

        timeUtcText.text = "Глобальное UTC время: \t" + dateTime;
        DateTime dt = DateTime.Parse(dateTime.ToString());
        print(dt.ToString("H:mm"));
        timePhoneText.text = "Время на устройстве: \t" + date + "\t" + actualTime;
    }

    IEnumerator updateRatios() // Обновляем каждые 15 секунд значения коэффициентов и скорости роста
    {
        while (true)
        {
            updateWaterRatio();
            updateLandRatio();
            updateSunRatio();
            updateHealthRatio();
            waterRatioText.text = "Коэффициент воды: \t" + waterRatio;
            landRatioText.text = "Коэффициент земли: \t" + landRatio;
            sunRatioText.text = "Коэффициент света: \t" + sunRatio;
            healthRatioText.text = "Коэффициент здоровья: \t" + healthRatio;
            updateGrowSpeed();

            

            yield return new WaitForSeconds(15);
        }
    }

    public void updateGrowSpeed()
    {
        ratios = waterRatio * landRatio * sunRatio * healthRatio;
        growSpeed = oak * speeder * ratios / 31536000; // Получаем скорость роста дерева в метрах/секунду
        growSpeedText.text = "Скорость роста: \t" + growSpeed.ToString("G20") + "\t м/с";
    }
    public void cheat()
    {
        treeHeight += 0.3f;
    }

    public void FixedUpdate()
    {
        treeHeight += growSpeed;

        if (treeHeight < 0.001)
        {
            float tempTH = treeHeight;
            tempTH *= 1000000;
            treeHeightText.text = "Высота дерева: \t" + tempTH.ToString("G20") + "\t микрометров";
        }
        else if (treeHeight > 0.001 && treeHeight < 0.01)
        {
            float tempTH = treeHeight;
            tempTH *= 1000;
            treeHeightText.text = "Высота дерева: \t" + tempTH.ToString("G20") + "\t миллиметров";
        }
        else if (treeHeight > 0.01 && treeHeight < 1)
        {
            float tempTH = treeHeight;
            tempTH *= 100;
            treeHeightText.text = "Высота дерева: \t" + tempTH.ToString("G20") + "\t сантиметров";
        }
        else
        {
            float tempTH = treeHeight;
            treeHeightText.text = "Высота дерева: \t" + tempTH.ToString("G20") + "\t метров";
        }
    }

    void Update()
    {

    }

    public void updateWaterRatio()
    {
        /*
        float water = 100; // Числитель дроби
        if (есть полив)
        {
            water += размер полива;
            if (water > 100)
            {
                water = 100;
            } else if (water < 0)
            {
                water = 0;
            }
        }
        else
        {
            water -= испарение + расход дерева;
            if (water > 100)
            {
                water = 100;
            }
            else if (water < 0)
            {
                water = 0;
            }
        } */
        // waterIncrease = waterIncreases[index];

        float water = 100; // Числитель дроби
        waterRatio = water / 100;
    }

    bool watering()
    {
        return true;
    }



    public float land = 100;
    public void fertilize()
    {
        if (land < 130)
        {
            for (int i = 1; i < 5; i++)
            {
                land += 1.25f;
                updateLandRatio();
                landRatioText.text = "Коэффициент земли: \t" + landRatio;
            }
        }
        else if (land >= 130)
        {
            ill();

        }
        updateGrowSpeed();
    }
    public void updateLandRatio()
    {
        landRatio = land / 100;
        print(landRatio);
    }

    public void ill()
    {
        health -= 5;
        updateHealthRatio();
        healthRatioText.text = "Коэффициент здоровья: \t" + healthRatio;
    }



    public void updateSunRatio()
    {
        float sun = 100;
        sunRatio = sun / 100;
        print(sunRatio);
    }

    public float health = 100;
    public void updateHealthRatio()
    {
        if (health <= 0)
            health = 0;
        if (health == 0)
            gameOver();
        healthRatio = health / 100;
        print(healthRatio);
    }
    public GameObject gameover;

    public void gameOver() //Конец игры, дерево умерло
    {
        gameover.SetActive(!gameover.activeSelf);
    }

    DateTime CheckGlobalTime() //Получаеем время UTC
    {
        var www = new WWW("https://google.com");
        while (!www.isDone && www.error == null)
            Thread.Sleep(1);

        var str = www.responseHeaders["Date"];
        DateTime dateTime;

        if (!DateTime.TryParse(str, out dateTime))
            return DateTime.MinValue;

        return dateTime.ToUniversalTime();
    }
}
