using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//�A�j���[�V������������UI(�{�^���Ȃ�)�ɃA�^�b�`����

public class ButtonAnimation : MonoBehaviour
{
    private void Start()
    {
        ChangeUISize();
    }

    private void ChangeUISize()
    {
        transform.DOScale(0.1f, 1f)
        .SetRelative(true)
        .SetEase(Ease.OutQuart)
        .SetLoops(-1, LoopType.Restart);
    }
}
