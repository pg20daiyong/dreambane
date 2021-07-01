using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //loads scenes with a string
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
