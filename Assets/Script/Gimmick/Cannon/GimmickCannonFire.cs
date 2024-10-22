using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   ��C�{�̂ɕt����X�N���v�g
 * 
 * @memo    �E������ƃv���C���[����u�����ɂȂ�
 *          �E�����ɂȂ�����莞�Ԍ��CannonProjectile�ɂ̂��Ĕ�΂�
 *          �E��΂��Ƃ��Ƀv���C���[�̃R���|�[�l���g�������������ɂ���
 */
public class GimmickCannonFire : MonoBehaviour
{
    public GameObject barrel;  ///< ���ˌ��ƂȂ�q�I�u�W�F�N�g
    public GameObject projectilePrefab;  ///< ���˂��鋅�̃v���n�u
    public float delayBeforeFire = 0.5f;  ///< ���˂܂ł̒x�����ԁi�f�t�H���g 0.5�b�j
    public float projectileSpeed = 10.0f;  ///< ���̑��x

    private GameObject player;  ///< �v���C���[��Ǐ]�����邽�߂̎Q��

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ������
            player = other.gameObject;
            player.GetComponent<Renderer>().enabled = false;

            // ���˂̒x���������J�n
            Invoke("FireProjectile", delayBeforeFire);
        }
    }

    private void FireProjectile()
    {
        // ������������
        player.GetComponent<Renderer>().enabled = true;

        // ���ˌ����狅�𐶐����A�����Ă�������ɔ���
        GameObject projectile = Instantiate(projectilePrefab, this.transform.position, barrel.transform.rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        // �����̌v�Z
        Vector3 vec = (barrel.transform.position - projectile.transform.position).normalized * projectileSpeed;
        projectileRb.AddForce(vec, ForceMode2D.Impulse);

        // �v���C���[�����ɒǏ]������
        player.transform.parent = projectile.transform;  // ���̎q�I�u�W�F�N�g�Ƃ��ăv���C���[��Ǐ]������
        player.transform.position = projectile.transform.position;  // ���̈ʒu�Ƀv���C���[��TP
        player.GetComponent<Rigidbody2D>().simulated = false;       // �d�͂ŗ����Ȃ��悤�ɂ���
        player.GetComponent<Collider2D>().enabled = false;          // �v���C���[�̓����蔻��Ŏ~�܂�Ȃ��悤�ɂ���

        // �����ǂɓ�������������鏈��
        GimmickCannonProjectile projectileScript = projectile.GetComponent<GimmickCannonProjectile>();
        if (projectileScript != null)
        {
            projectileScript.SetPlayerFollowedObject(player);  // �v���C���[�̎Q�Ƃ�n��
        }
    }
}
