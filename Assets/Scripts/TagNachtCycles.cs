using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TagNachtCycles : MonoBehaviour
{
    public SpriteRenderer tagSprite;
    public SpriteRenderer nachtSprite;
    public SpriteRenderer nachtLayer;
    public SpriteRenderer uhr;

    public List<Sprite> UhrSprites;

    public bool isDay;
    public int nightCount;
    bool cycleOn = false;
    public float dayTimeInSeconds;

    bool active = false;

    public EnemySpawner enemySpawner;

    public AudioSource musicDay;
    public AudioClip Day;
    public AudioClip Night;

    // Start is called before the first frame update
    void Awake()
    {
        nightCount = 0;
        //UhrSprites = new List<Sprite>();
        musicDay.PlayOneShot(Day);
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        tagSprite.enabled = false;
        nachtSprite.enabled = false;
        nachtLayer.enabled = false;
        if (isDay)
        {
            tagSprite.enabled = true;
            
        }
        else
        {
            nachtSprite.enabled = true;
            nachtLayer.enabled = true;
            
        }
    }

    private void StartCycle()
    {
        nightCount = 0; 
        if (!cycleOn)
            StartCoroutine(DayNightRun());
    }
    private void StopCycle()
    {
        StopCoroutine(DayNightRun());
        nightCount = 0;
        cycleOn = false;
    }

    IEnumerator DayNightRun()
    {
        cycleOn = true;
        int i = 0;
        while (true)
        {
            
            uhr.sprite = UhrSprites[i];
            i++;
            if(i>=UhrSprites.Count)
            {
                i = 0;
                isDay = !isDay;
                //player.switchMaterial();
                if (!isDay)
                {
                    nightCount++;
                    musicDay.Stop();
                    musicDay.PlayOneShot(Night);
                }
                else
                {
                    musicDay.Stop();
                    musicDay.PlayOneShot(Day);
                    enemySpawner.clearMousesCounter();
                }
            }
            yield return new WaitForSeconds(dayTimeInSeconds);
        }
    }

    public void setActive(bool val)
    {
        if (val == active)
            return;

        active = val;
        if (!active)
            StopCycle();
        else
            StartCycle();
    }
}
