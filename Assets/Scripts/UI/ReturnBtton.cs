using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnBtton : MonoBehaviour
{
    public int returnSceneId = 0;

    public void ReturnTo()
    {
        SceneManager.LoadScene(returnSceneId);
    }
}