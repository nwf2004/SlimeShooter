using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //Start game button
    public void loadMainGame()
    {
        SceneManager.LoadScene(1);
    }

    //Quit game button
    public void quitGame()
    {
        Application.Quit();
    }

}
