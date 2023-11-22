using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Material cum;

    private int money = 100;

    public static float BuffVal = 1;

    [SerializeField] private GameObject GameUI;
    [SerializeField] private GameObject GameOverUI;
    public int Money
    {
        get { return money; }
        set
        {
            money = Mathf.Max(value, 0);
            uiManager.Init();
        }
    }
    public int Killed = 0;

    public UIManager uiManager;

    public void whiteWashing(GameObject go)
    {
        StartCoroutine(WhiteWash(go));
    }

    IEnumerator WhiteWash(GameObject go)
    {
        SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
        Material defaulMaterial = renderer.material;
        renderer.material = cum;
        yield return new WaitForSeconds(.1f);
        renderer.material = defaulMaterial;
    }

    public void GameOver()
    {
        GameUI.SetActive(false);
        GameOverUI.SetActive(true);
        print("OK I WILL");
    }

}
