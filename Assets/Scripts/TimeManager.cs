using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public UIManager ui;

    private static int time = 300;

    public SunHandler sun;

    public static TimeManager Instance;

    public bool IsDay
    {
        get
        {
            return time < 1020 && time > 360;
        }
    }

    IEnumerator Start()
    {
        Instance = new TimeManager();
        time = 300;
        while (true)
        {
            time += 15;
            if (time >= 1440)
            {
                time = 0;
            }
            var hour = time / 60;
            var min = time % 60;
            ui.UpdateTime(string.Format("{0}:{1}", hour.ToString("00"), min.ToString("00")));
            yield return new WaitForSeconds(1.25f);
        }
    }
}