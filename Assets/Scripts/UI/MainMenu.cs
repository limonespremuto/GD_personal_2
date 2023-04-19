using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string dungeon1Name;
    

    public void ToDungeon()
    {
        SceneManager.LoadScene(dungeon1Name);
    }

    public void Exit()
    {
        Application.Quit();
        
    }
}
