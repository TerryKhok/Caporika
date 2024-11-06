using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform frame;    // �{�^���g
    private RectTransform button;//�{�^��

    public Vector2 selectedPositionOffset = new Vector2(-5, 5); // �I�����̈ʒu�I�t�Z�b�g
    public Vector2 pressOffset = new Vector2(5, -5); // �I�����̈ʒu�I�t�Z�b�g

    private Vector2 originalFramePosition;  // �{�^���g�̌��̈ʒu
    private Vector2 originalButtonPosition;  // �{�^���g�̌��̈ʒu

    private void Start()
    {
        button = this.GetComponent<RectTransform>();
        // �{�^���ƃ{�^���g�̌��̈ʒu���L�^
        originalFramePosition = frame.anchoredPosition;
        originalButtonPosition = button.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // �|�C���^�[���{�^���ɏd�Ȃ����Ƃ��̏���
        SoundManager.Instance.PlaySE("MENU_MOVE");
        frame.anchoredPosition = originalFramePosition + selectedPositionOffset;   // �{�^���g�̈ʒu�𒲐�
        button.anchoredPosition = originalButtonPosition + selectedPositionOffset;   // �{�^���̈ʒu�𒲐�
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // �|�C���^�[���{�^�����痣�ꂽ�Ƃ��̏���
        frame.anchoredPosition = originalFramePosition;   // �{�^���g�̈ʒu�����ɖ߂�
        button.anchoredPosition = originalButtonPosition;   // �{�^���̈ʒu�����ɖ߂�
    }

    // �{�^���������ꂽ�Ƃ�
    public void OnPointerDown(PointerEventData eventData)
    {
        // �{�^��������ɉ����A�e�Əd�Ȃ�悤�ɂ���
        SoundManager.Instance.PlaySE("MENU_SELECT");
        frame.localPosition = originalFramePosition + pressOffset;
        button.localPosition = originalButtonPosition + pressOffset;
    }

    // �{�^����������Ȃ��Ȃ����Ƃ�
    public void OnPointerUp(PointerEventData eventData)
    {
        // �{�^��������ɉ����A�e�Əd�Ȃ�悤�ɂ���
        frame.localPosition = originalFramePosition + selectedPositionOffset;
        button.localPosition = originalButtonPosition + selectedPositionOffset;
    }


}
