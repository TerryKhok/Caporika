using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIOptionButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    private RectTransform button;//�{�^��
    public GameObject shadowImage;
    public Vector2 selectedPositionOffset = new Vector2(-11, 11); // �I�����̈ʒu�I�t�Z�b�g
    private Vector2 originalButtonPosition;  // �{�^���g�̌��̈ʒu
    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<RectTransform>();
        // �{�^���̌��̈ʒu���L�^
        originalButtonPosition = button.anchoredPosition;
        shadowImage.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // �|�C���^�[���{�^���ɏd�Ȃ����Ƃ��̏���
        button.anchoredPosition = originalButtonPosition + selectedPositionOffset;   // �{�^���̈ʒu�𒲐�
        shadowImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // �|�C���^�[���{�^�����痣�ꂽ�Ƃ��̏���
        button.anchoredPosition = originalButtonPosition;   // �{�^���̈ʒu�����ɖ߂�
        shadowImage.SetActive(false);
    }

    // �{�^���������ꂽ�Ƃ�
    public void OnPointerDown(PointerEventData eventData)
    {
        // �{�^��������ɉ����A�e�Əd�Ȃ�悤�ɂ���
        button.localPosition = originalButtonPosition;
    }

    // �{�^����������Ȃ��Ȃ����Ƃ�
    public void OnPointerUp(PointerEventData eventData)
    {
        // �{�^��������ɉ����A�e�Əd�Ȃ�悤�ɂ���
        button.localPosition = originalButtonPosition + selectedPositionOffset;
    }
}
