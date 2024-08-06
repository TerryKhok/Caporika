using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/**
 *  @brief 	�v���C���[�̊Ǘ�
 *  
 *  @memo   �E�c�@�̊Ǘ�
 *          �E��������}�g�����[�V�J
 *          �E���ʂƂ��̏���
*/
public class MatryoshkaManager : MonoBehaviour
{
    public int maxLife;                     // �c�@
    public GameObject[] matryoshkaPrefabes; // ��������}�g�����[�V�J�̃v���n�u

    private int currentLife = 0;            // ���݂̎c�@

    // Start is called before the first frame update
    void Start()
    {
        // �c�@���Z�b�g
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        // �c�@��0�̎�
        if(currentLife<=0)
        {
            // �Q�[���I�[�o�[
            GameOver();
        }
    }


    /**
     *  @brief 	�c�@�𑝂₷
    */
    public void AddLife()
    {
        if(currentLife<= maxLife)
        {
            currentLife++;
        }
    }

    /**
     *  @brief  �c�@�����炷
    */
    public void LoseLife()
    {
        if (currentLife >0)
        {
            currentLife--;
        }
    }

    /**
     *  @brief  �w��̃}�g�����[�V�J�𐶐�
     *  @param  int         _index          ��������T�C�Y(�v�f�ԍ�)
     *  @return GameObject  createPrefab    ���������I�u�W�F�N�g
    */
    public GameObject InstanceMatryoshka(int _index)
    {
        GameObject createPrefab = null;
        createPrefab = Instantiate(matryoshkaPrefabes[_index]);
        return createPrefab;
    }

    /**
     *  @brief  ���񂾂Ƃ��̏���
    */
    private void GameOver()
    {
        Debug.Log("GameOver");
    }
}
