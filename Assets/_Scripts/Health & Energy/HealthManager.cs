using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class HealthManager : MonoBehaviour
{
    [SerializeField] TextMeshPro ballCount;
    [SerializeField] Transform healthbar;
    Stack<int> balls = new Stack<int>();

    public int currentHealth;
    float maxHealthbarScale;

    public static event Action BallLoaded;

    private void OnEnable() {
        Ball.DeathLineTouched += UseBall;
        Hitter.DamageTaken += RemoveHealth;

        maxHealthbarScale = healthbar.transform.localScale.x;
        AddBall(3);
    }

    private void OnDisable() {
        Ball.DeathLineTouched -= UseBall;
        Hitter.DamageTaken -= RemoveHealth;
    }

    private void Start() {
        UseBall();
    }

    void SetHealthbar(int amount){
        healthbar.transform.DOScaleX(maxHealthbarScale * (amount / 100f), 1f);
    }

    void RemoveHealth(int amount){
        currentHealth -= amount;
        if(currentHealth <= 0){
            UseBall();
            return;
        }
        SetHealthbar(currentHealth);
    }

    void AddHealth(int amount){
        currentHealth = Mathf.Min(currentHealth + amount, 100);
        SetHealthbar(currentHealth);
    }

    void UseBall(){
        if(balls.Count > 0){
            BallLoaded?.Invoke();
            AddHealth(balls.Pop());
            UpdateCounter(); 
        }else{
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void StoreBall(){
        balls.Push(currentHealth);
    }

    void AddBall(int amount = 1){
        for (int i = 0; i < amount; i++)
        {
            balls.Push(100);
        }
        UpdateCounter();
    }

    void RemoveBall(){
        balls.Pop();
        UpdateCounter();   
    }

    void UpdateCounter(){
        ballCount.text = balls.Count.ToString();
    }

    //DEBUG
    [ContextMenu("Remove 10 Health")]
    void Remove10Health(){
        RemoveHealth(10);
    }
    [ContextMenu("Remove 100 Health")]
    void Remove100Health(){
        RemoveHealth(100);
    }
    [ContextMenu("Add 10 Health")]
    void Add10Health(){
        AddHealth(10);
    }
}
