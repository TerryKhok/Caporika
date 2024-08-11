using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	�L�����̑傫���A�d����ێ�����
 * 
 *  @memo   Rigidbody2D.Mass�ɏd����K�p��������悤�Ɉꉞ�ύX�\(���͓K�p�~)
*/
public class CharaState : MonoBehaviour
{
    [SerializeField]
    private int size;   // �L�����̑傫��

    [SerializeField]
    private int weight; // �L�����̏d��

    private void Start()
    {
        //// rigidbody2D.Mass�ɏd����K�p����
        //Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //if(!rb)
        //{
        //    Debug.LogError("Rigidbody2D��������܂���ł����B");
        //    return;
        //}
        //rb.mass = this.weight;
    }

    /**
     *  @brief 	�L�����̏d���̃Z�b�g
     *  @param int _charaWeight  �d��
    */
    public void SetCharaWeight(int _charaWeight)
    {
        this.weight= _charaWeight;
    }

    /**
     *  @brief 	�L�����̏d���̎擾
     *  @return int this.weight  �d��
    */
    public int GetCharaWeight()
    {
        return this.weight;
    }

    /**
     *  @brief 	�L�����̑傫���̃Z�b�g
     *  @param int _charaSize  �傫��
    */
    public void SetCharaSize(int _charaSize)
    {
        this.size = _charaSize;
    }

    /**
     *  @brief 	�L�����̑傫���̎擾
     *  @return int this.size  �傫��
    */
    public int GetCharaSize()
    {
        return this.size;
    }
}