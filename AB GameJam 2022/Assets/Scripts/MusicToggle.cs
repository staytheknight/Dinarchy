using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicToggle : MonoBehaviour
{

    [SerializeField] GameObject rightMusic;
    [SerializeField] GameObject leftMusic;
    [SerializeField] GameObject topMusic;
    [SerializeField] bool invertedEnemy;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("InvertKill"))
        {
            invertedEnemy = !invertedEnemy;
        }
        if (Input.GetButtonUp("InvertKill"))
        {
            invertedEnemy = !invertedEnemy;
        }

        if (!invertedEnemy)
        {
            topMusic.SetActive(false);
            rightMusic.SetActive(true);
            leftMusic.SetActive(true);
        }

        if (invertedEnemy)
        {
            topMusic.SetActive(true);
            rightMusic.SetActive(false);
            leftMusic.SetActive(false);
        }

    }
}