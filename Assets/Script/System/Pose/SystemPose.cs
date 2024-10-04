using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPose : MonoBehaviour
{
    public KeyCode  PoseKey = KeyCode.Escape;          // Poseを行うキー
    bool is_poseNow = false;
    Canvas poseCanvas;
    private GameObject OptionCanvas;
    private Canvas OptionCanv;
    
    void Start()
    {
        poseCanvas = this.GetComponent<Canvas>();
        poseCanvas.enabled = false;
        OptionCanvas = GameObject.Find("OptionCanv");
        OptionCanv = OptionCanvas.GetComponent<Canvas>();
    }

    void Update()
    {
        if (Input.GetKeyDown(PoseKey))
        {
            if (is_poseNow == false)    //PoseFgがfalseなら時間を止める
            {
                PoseStart();
            }
            else
            {
                PoseEnd();
            }
        }
    }

    public void PoseStart()
    {
        poseCanvas.enabled = true;
        is_poseNow = true;
        Time.timeScale = 0.0f;
    }
    public void PoseEnd()
    {
        poseCanvas.enabled = false;
        is_poseNow = false;
        OptionCanv.enabled = false;  //OptionCanvasを非表示
        Time.timeScale = 1.0f;
    }
}
