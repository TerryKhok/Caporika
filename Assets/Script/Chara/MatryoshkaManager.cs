using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/**
 *  @brief 	�v���C���[�̊Ǘ�
 *  
 *  @memo   �E�c�@�̊Ǘ�
 *          �E��������}�g�����[�V�J
 *          �E���ʂƂ��̏���
 *          �E�X�^�[�g���Ƀ}�g�����V�J���`�F�b�N�|�C���g�ɐ���
 *          
 *          ���`�F�b�N�|�C���g��0�Ԃ̓X�^�[�g�n�_�ł��B
*/
public class MatryoshkaManager : MonoBehaviour
{
    public int maxLife;                     // �c�@
    public GameObject[] matryoshkaPrefabes; // ��������}�g�����[�V�J�̃v���n�u

    private int currentLife = 0;            // ���݂̎c�@

    [SerializeField] private GameObject[] checkpoints;  // �`�F�b�N�|�C���g

    // Start is called before the first frame update
    void Start()
    {
        // �c�@���Z�b�g
        currentLife = maxLife;

        // �}�g�����V�J�𐶐����ă`�F�b�N�|�C���g�Ɉړ�
        var firstMatryoshka = InstanceMatryoshka(currentLife);
        firstMatryoshka.gameObject.transform.position = checkpoints[GimmickCheckpointParam.GetCheckpointNum()].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // �c�@��0�̎�
        if (currentLife <= 0)
        {
            // �Q�[���I�[�o�[
            GameOver();
        }
    }

    /**
     *  @brief 	�c�@�����擾����
    */
    public int GetCurrentLife()
    {
        return currentLife;
    }

    /**
     *  @brief 	�c�@�𑝂₷
    */
    public void AddLife()
    {
        if (currentLife <= maxLife)
        {
            currentLife++;
        }
    }

    /**
     *  @brief  �c�@�����炷
     *  @return int ���݂̎c�@
    */
    public int LoseLife()
    {
        if (currentLife > 0)
        {
            currentLife--;
        }
        return currentLife;
    }

    /**
     *  @brief  �w��̃}�g�����[�V�J�𐶐�
     *  @param  int         _index          ��������T�C�Y(�v�f�ԍ�)
     *  @return GameObject  createPrefab    ���������I�u�W�F�N�g
    */
    public GameObject InstanceMatryoshka(int _index)
    {
        GameObject createPrefab = null;
        if (_index > 0)
        {
            createPrefab = Instantiate(matryoshkaPrefabes[_index - 1]);
        }
        return createPrefab;
    }

    /**
     *  @brief  ���񂾂Ƃ��̏���
    */
    public void GameOver()
    {
        Debug.Log("GameOver");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}