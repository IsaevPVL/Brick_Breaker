using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager active { get; private set; }

    [SerializeField] GameObject bar;
    [SerializeField] Transform energyBarsStart;
    [SerializeField] int maxBarsAmount;
    [SerializeField] float padding;

    //TEMP
    public int energyToAdd;
    public int barsToUse;

    public int chargedBars = 0;
    public int currentChargingLevel = 0;
    GameObject[] energyBars;
    float scaleX;

    private void Awake()
    {
        if (active != null && active != this)
        {
            Destroy(this);
        }
        else
        {
            active = this;
        }

        CalculateScaleAndPosition();
        StartCoroutine(DecreaseEnergyBarsVisuals());
    }

    public void AddEnergy(int amount)
    {
        currentChargingLevel = Mathf.Max(0, currentChargingLevel + amount);
        if (currentChargingLevel >= 100 && chargedBars < maxBarsAmount)
        {
            for (int i = 0; i < currentChargingLevel / 100; i++)
            {
                chargedBars++;
                if (chargedBars == maxBarsAmount)
                {
                    currentChargingLevel = 0;
                    break;
                }
            }
            currentChargingLevel %= 100;
        }else if(chargedBars == maxBarsAmount){
            currentChargingLevel = 0;
        }
        StartCoroutine(IncreaseEnergyBarsVisuals());
    }

    IEnumerator IncreaseEnergyBarsVisuals()
    {
        for (int i = 0; i < chargedBars; i++)
        {
            yield return StartCoroutine(SetBar(i, 100));
        }
        if (chargedBars < maxBarsAmount)
        {
            yield return StartCoroutine(SetBar(chargedBars, currentChargingLevel));
        }
    }

    IEnumerator DecreaseEnergyBarsVisuals()
    {
        for (int i = maxBarsAmount - 1; i >= chargedBars; i--)
        {   
            if(i == chargedBars && currentChargingLevel > 0){
                yield return StartCoroutine(SetBar(chargedBars, currentChargingLevel));
                yield break;
            }
            yield return StartCoroutine(SetBar(i, 0));
        }
    }

    IEnumerator SetBar(int barIndex, float percent)
    {
        yield return energyBars[barIndex].transform.DOScaleX(percent / 100f, 0.5f).WaitForCompletion();
    }

    public bool CheckIfEnergyBarsAvailable(int amount)
    {
        if (amount <= chargedBars)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void UseEnergyBars(int amount)
    {
        if (CheckIfEnergyBarsAvailable(amount))
        {
            chargedBars -= amount;
            StartCoroutine(DecreaseEnergyBarsVisuals());
        }
    }

    void CalculateScaleAndPosition()
    {
        scaleX = (GetComponent<BoxCollider>().bounds.size.x - padding * (maxBarsAmount + 1)) / maxBarsAmount;
        energyBars = new GameObject[maxBarsAmount];
        for (int i = 0; i < maxBarsAmount; i++)
        {
            energyBars[i] = Instantiate(bar, energyBarsStart);
            energyBars[i].transform.localPosition = new Vector3(padding + padding * i + scaleX * i, padding, -0.01f);
            energyBars[i].transform.localScale = new Vector3(scaleX, energyBars[i].transform.localScale.y, energyBars[i].transform.localScale.z);
        }
    }

    //DEBUG
    [ContextMenu("Add Energy")]
    void AddEnergyTest()
    {
        AddEnergy(energyToAdd);
    }

    [ContextMenu("Use Energy Bars")]
    void UseEnergyBarsTest()
    {
        UseEnergyBars(barsToUse);
    }
}