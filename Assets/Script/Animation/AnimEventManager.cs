using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventManager : MonoBehaviour
{
     [SerializeField] private GameObject irisLid;
    // Start is called before the first frame update
    public void IrisOutFunction()
    {
        Debug.Log(irisLid);
        irisLid.SetActive(true);
    }
}
