using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : BasePlatform
{
    public Vector2 StartPos;
    public Vector2 EndPos;
    public Vector2 Difference;

    float Seconds = 2;
    float WaitTime = 1;

    float Timer;
    
    float Percent;

    public static int MIN_RANGE = 1;
    public static int MAX_RANGE = 4;

    bool isIncrease = true;

    void Start()
    {
        StartPos = transform.position;
        int roll = Random.Range(0, 3);
        switch (roll)
        {
            case 0:
                EndPos.x = StartPos.x + Random.Range(MIN_RANGE, MAX_RANGE);
                EndPos.y = transform.position.y;
                break;
            case 1:
                EndPos.x = transform.position.x;
                EndPos.y = StartPos.y + Random.Range(MIN_RANGE, MAX_RANGE);
                break;
            case 2:
                EndPos.x = StartPos.x + Random.Range(MIN_RANGE, MAX_RANGE);
                EndPos.y = StartPos.y + Random.Range(MIN_RANGE, MAX_RANGE);
                break;
        }

        while(EndPos.x > Camera.main.aspect)
        {
            EndPos.x -= 1;
        }


        Difference = EndPos - StartPos;
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if (isIncrease)
        {
            if (Timer <= Seconds + WaitTime)
            {
                Timer += Time.deltaTime;

                Percent = Timer <= Seconds ? Timer / Seconds : 1;

                transform.position = StartPos + Difference * Percent;
            }
            else
            {
                isIncrease = false;
                Timer = Seconds;
            }
        }
        else
        {
            if (Timer >= 0 - WaitTime)
            {
                Timer -= Time.deltaTime;

                Percent = Timer >= 0 ? Timer / Seconds : 0;

                transform.position = StartPos + Difference * Percent;
            }
            else
            {
                isIncrease = true;
                Timer = 0;
            }
        }
    }
}
