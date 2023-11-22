using Assets.Scripts.PowerUp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class SelectionHandler : MonoBehaviour
{
    private System.Random rand = new System.Random();
    private double accumulatedWeights;
    private double accumTemp;

    UIDocument _uiDocument;
    private string[] choices;
    private VisualElement[] selections = new VisualElement[3];

    [SerializeField] private PowerUpEffect[] powerUpEffects;
    private PowerUpEffect[] PowerUpEffectsTemp;

    private PowerUpEffect[] selected = new PowerUpEffect[3];

    VisualElement root;
    
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        root = _uiDocument.rootVisualElement;
        CalculateWeights();
    }

    private void Start()
    {
        for (int i = 0; i <= 2; i++)
        {
            if (root == null) continue;
            selections[i] = root.Q<VisualElement>($"selection{i + 1}");
            int index = i;
            selections[i].RegisterCallback<ClickEvent>((evt) => OnClick(index)); ;
        }
    }

    private void OnClick(int index)
    {
        selected[index].Apply(Player.Instance.gameObject);
        Player.Instance.state = Player.State.None;

        //Hades_Transition.Instance.targetValue = 1f;

        WaveSpawner.Instance.waitingForInput = false;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        root = _uiDocument.rootVisualElement;
        Roll();
        SetText();
        for (int i = 0; i <= 2; i++)
        {
            if (root == null) continue;
            selections[i] = root.Q<VisualElement>($"selection{i + 1}");
            int index = i;
            selections[i].RegisterCallback<ClickEvent>((evt) => OnClick(index)); ;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i <= 2; i++)
        {
            if (root == null) continue;
            int index = i;
            // 콜백을 해제합니다.
            selections[i].UnregisterCallback<ClickEvent>((evt) => OnClick(index));
        }
    }

    private void Roll()
    {
        PowerUpEffectsTemp = powerUpEffects.ToArray();
        accumTemp = accumulatedWeights;
        List<int> selectedIndices = new List<int>();

        for (int i = 0; i <= 2; i++)
        {
            int selectedIndex;
            do
            {
                selectedIndex = GetRandomEnemyIndex(PowerUpEffectsTemp, accumTemp);
            } while (selectedIndices.Contains(selectedIndex));

            selectedIndices.Add(selectedIndex);

            selected[i] = PowerUpEffectsTemp[selectedIndex];
            accumTemp -= selected[i].Chance;
            PowerUpEffectsTemp[selectedIndex] = null; // 선택된 파워업 제거
        }
    }

    private void SetText()
    {
        if (root == null) return;
        for (int i = 0; i <= 2; i++)
        {
            root.Q<Label>($"ability-name{i + 1}").text = selected[i].name;//GetType().Name;
            root.Q<Label>($"description{i + 1}").text = selected[i].Desc;
        }
    }

    private int GetRandomEnemyIndex(PowerUpEffect[] PowerUpEffectsTemp, double accum)
    {
        double r = rand.NextDouble() * accum;

        for (int i = 0; i < PowerUpEffectsTemp.Length; i++)
        {
            if (PowerUpEffectsTemp[i] == null) continue;
            if (PowerUpEffectsTemp[i]._weight >= r)
                return i;
        }
        //print("d");
        return 0;
    }

    private void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (PowerUpEffect power in powerUpEffects)
        {
            accumulatedWeights += power.Chance;
            power._weight = accumulatedWeights;
        }
    }
}
