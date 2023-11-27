using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject menu;
    public void toggleOpen()
    {
        menu.SetActive(!menu.activeSelf);
        if (!menu.activeSelf)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0;
    }
    public void Quit()
    {
        Application.Quit();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            toggleOpen();

        }
    }
}
