using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public string bgmID = "MENU_BGM";

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(bgmID);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
