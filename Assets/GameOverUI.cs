using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text Waves;
    [SerializeField] private TMP_Text Coins;
    [SerializeField] private TMP_Text Enemies;

    private void OnEnable()
    {
        Waves.text = $"Survived Waves : {WaveSpawner.Instance.TotalWave}";
        Coins.text = $"Coins : {GameManager.Instance.Money}";
        Enemies.text = $"Killed Enemies : {GameManager.Instance.Killed}";
        
        string fileName = "MingLog.txt";

        try
        {
            File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName), $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt")} Record : {WaveSpawner.Instance.TotalWave} waves");
        }
        catch (Exception ex)
        {
            Debug.LogError($"err : {ex}");
        }
    }
}
