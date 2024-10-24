using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimGoalEvent : MonoBehaviour
{
    GameObject resultObj;
    Animator resultAnim;
    public void Start()
    {
        resultObj = GameObject.Find("ResultBase");
        resultAnim = resultObj.GetComponent<Animator>();
    }

    public void GoalAnimFinishEvent()
    {
        resultAnim.Play("ResultAnim");
    }
}
