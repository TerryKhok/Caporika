using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief  親オブジェクトにトリガー判定を渡す
 *  @memo   SendMessageOptions.DontRequireReceiver を指定することで、親オブジェクトがこのメソッドを持っていない場合でもエラーが発生しないので注意！！！
*/
//public class ChildTriggerNotifier : MonoBehaviour
//{
//    void OnTriggerStay2D(Collider2D _other)
//    {
//        // 親オブジェクトに通知
//        // OnChildTriggerEnter2Dメソッドに_otherを引数として渡す
//        this.transform.parent.SendMessage("OnChildTriggerStay2D", _other, SendMessageOptions.DontRequireReceiver);
//    }
//}