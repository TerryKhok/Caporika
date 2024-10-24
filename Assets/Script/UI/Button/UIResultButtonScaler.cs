using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIResultButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform button;//�{�^��

    public Vector2 selectedPositionOffset = new Vector2(5, -5); // �I�����̈ʒu�I�t�Z�b�g
    public Vector2 pressOffset = new Vector2(10, -10); // �I�����̈ʒu�I�t�Z�b�g
    private Vector2 originalButtonPosition;  // �{�^���g�̌��̈ʒu
    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<RectTransform>();
        // �{�^���̌��̈ʒu���L�^
        originalButtonPosition = button.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // �|�C���^�[���{�^���ɏd�Ȃ����Ƃ��̏���
        button.anchoredPosition = originalButtonPosition + selectedPositionOffset;   // �{�^���̈ʒu�𒲐�
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // �|�C���^�[���{�^�����痣�ꂽ�Ƃ��̏���
        button.anchoredPosition = originalButtonPosition;   // �{�^���̈ʒu�����ɖ߂�
    }

    // �{�^���������ꂽ�Ƃ�
    public void OnPointerDown(PointerEventData eventData)
    {
        // �{�^��������ɉ����A�e�Əd�Ȃ�悤�ɂ���
        button.localPosition = originalButtonPosition + pressOffset;
    }

    // �{�^����������Ȃ��Ȃ����Ƃ�
    public void OnPointerUp(PointerEventData eventData)
    {
        // �{�^��������ɉ����A�e�Əd�Ȃ�悤�ɂ���
        button.localPosition = originalButtonPosition + selectedPositionOffset;
    }
}
