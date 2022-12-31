using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    public UnityEvent endGoalReached;
    const float END_GOAL = 30;

    [SerializeField] private Transform energyBalken;
    const float FULL_ENERGY = 0.424593f;

    [SerializeField] private List<SpriteRenderer> healthData;
    [SerializeField] private List<SpriteRenderer> FullhealthData;
    const float FULL_HEALTH = 7;

    [SerializeField] private List<SpriteRenderer> waende;

    bool active = false;

    public Canvas canvas;
    public TextMeshProUGUI deadText;
    public TextMeshProUGUI liveText; 
    public TextMeshProUGUI endText;

    public int livingMouse = 0;
    public int deadMouse = 0;
    int score;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 startEnergy = new Vector3(FULL_ENERGY, energyBalken.localScale.y, 1f);
        energyBalken.localScale = startEnergy;
        FullhealthData = healthData;
        canvas.enabled = false;
    }

    void Update()
    {
        if (!active)
            return;

        score = deadMouse - livingMouse;
        if (score >= END_GOAL)
            endGoalReached.Invoke();

        deadText.text = deadMouse.ToString();
        liveText.text = livingMouse.ToString();
        endText.text = score.ToString();

    }

    public void setActive(bool val)
    {
        if (val == active)
            return;

        active = val;
        if (active)
            canvas.enabled = true;
        else
            canvas.enabled = false;

    }



    public void setEnergy(float amount)
    {
        Vector3 newEnergy = new Vector3(Mathf.Lerp(0, FULL_ENERGY, amount), energyBalken.localScale.y, 1f);
        energyBalken.localScale = newEnergy;
    }

    public void loseHearth(int health, float flashtime)
    {
        int i = 0;
        foreach(var heart in healthData)
        {
            heart.enabled = false;
            if (i < health)
                heart.enabled = true;

            i++;
        }
        FlashWalls(flashtime);
    }


    private IEnumerator flashCoroutine;
    private void FlashWalls(float flashtime)
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = DoFlash(flashtime);
        StartCoroutine(flashCoroutine);
    }

    private void SetFlashAmount(float flashAmount)
    {
        foreach(SpriteRenderer wand in waende)
        {
            wand.material.SetFloat("_FlashAmount", flashAmount);
        }
    }

    private IEnumerator DoFlash(float flashtime)
    {
        float lerpTime = 0;

        while (lerpTime < flashtime)
        {
            lerpTime += Time.deltaTime;
            float perc = lerpTime / flashtime;

            SetFlashAmount(1f - perc);
            yield return null;
        }
        SetFlashAmount(0);
    }


}
