using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickTrampoline : MonoBehaviour
{
    public Transform targetPoint = null;   // �_���[�W���̖ڕW�ʒu
    public float speed = 3.0f;       // �_���[�W���̈ړ��̑��x
    public float height = 2.0f;      // �_���[�W���̈ړ��̍����̍ő�l�i�V��ɂԂ���Ȃ����߁j

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // ���������̂��v���C���[���G�Ȃ�
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Enemy"))
        {
            List<string> soundList = new List<string> { "STAGE_TRAMPOLINE_1", "STAGE_TRAMPOLINE_2" };
            SoundManager.Instance.PlayRandomSE(soundList);
            // �������ړ��p�X�N���v�g�����Ĕ�΂�
            var move = collider.gameObject.AddComponent<ParabolicMove>();
            move.Init(targetPoint.transform, speed, height, true);
            move.Active();
        }
    }
}
