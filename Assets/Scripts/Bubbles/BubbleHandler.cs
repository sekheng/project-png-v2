using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleHandler : MonoBehaviour
{
    public GameObject[] bubbles;

    public Vector2 delayTime;

    int currentBubbleIndex = 0;

    public float touchSpeed;

    public bool pauseTouch;

    void Start()
    {
        touchSpeed = PlayerPrefs.GetFloat("BubblesTouchSpeed", 4);

        StartCoroutine(SpawnBubbles());
    }

    public void OnPauseResumeTouch(bool isPause)
    {
        pauseTouch = isPause;
    }

    IEnumerator SpawnBubbles()
    {
        float time = 0;
        while (true)
        {
            time = Random.Range(delayTime.x, delayTime.y);

            bubbles[currentBubbleIndex].SetActive(false);
            bubbles[currentBubbleIndex].SetActive(true);
            currentBubbleIndex = (currentBubbleIndex + 1) % bubbles.Length;

            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}