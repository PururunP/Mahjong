using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// タイトル操作
public class Title : MonoBehaviour
{
    // Startボタン押下
    public void doStartButton()
    {
        SceneManager.LoadScene("Main");
    }
}
