using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickCheckpointParam
{
    static int checkpointNum = 0;
    static int cameraNum = 0;

    public static void SetCheckpointNum(int _cp, int _cam)
    {
        if (_cp > checkpointNum)
        {
            checkpointNum = _cp;
            cameraNum = _cam;
        }
    }

    public static int GetCheckpointNum() { return checkpointNum; }
    public static int GetCameraNum() { return cameraNum; }

    public static void ResetCheckpointParams()
    {
        checkpointNum = 0;
        cameraNum = 0;
    }
}
