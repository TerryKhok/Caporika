using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject shadow;      // �e�I�u�W�F�N�g
    public RectTransform frame;    // �{�^���g

    public Vector2 selectedSize = new Vector2(579, 211);  // �I�����̃T�C�Y
    public Vector2 unselectedSize = new Vector2(579, 211); // ���I�����̃T�C�Y
    public Vector2 selectedPositionOffset = new Vector2(-11, 11); // �I�����̈ʒu�I�t�Z�b�g

    private Vector2 originalFramePosition;  // �{�^���g�̌��̈ʒu

    private void Start()
    {
        // �{�^���ƃ{�^���g�̌��̈ʒu���L�^
        originalFramePosition = frame.anchoredPosition;

        // ������Ԃł͉e���\��
        shadow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // �|�C���^�[���{�^���ɏd�Ȃ����Ƃ��̏���
        OnSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // �|�C���^�[���{�^�����痣�ꂽ�Ƃ��̏���
        OnDeselect();
    }

    private void OnSelect()
    {
        // �{�^���I�����̏���
        shadow.SetActive(true); // �e��\��
        frame.sizeDelta = selectedSize;  // �{�^���g�̃T�C�Y��ύX
        frame.anchoredPosition = originalFramePosition + selectedPositionOffset;   // �{�^���g�̈ʒu�𒲐�
    }

    private void OnDeselect()
    {
        // �{�^����I�����̏���
        shadow.SetActive(false); // �e���\��
        frame.sizeDelta = unselectedSize;  // �{�^���g�̃T�C�Y�����ɖ߂�
        frame.anchoredPosition = originalFramePosition;   // �{�^���g�̈ʒu�����ɖ߂�
    }
}
