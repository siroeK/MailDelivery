using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���C����ʂ̍ő勗���e�L�X�g�̃A�j���[�V����
/// </summary>
public class MaxDistanceAnimation : MonoBehaviour
{

    [Header("�g�k�A�j���[�V����(DOScale)")]
    [SerializeField, Tooltip("�ڕW�̑傫��")]
    private Vector3 targetScale = Vector3.one;
    [SerializeField, Tooltip("�T�C�Y�̊g�k���o����")]
    private float scaleProductionTime = 1.0f;
    [SerializeField, Tooltip("�T�C�Y�̊g�k�̌J��Ԃ��^�C�v")]
    private LoopType scaleLoopType = LoopType.Restart;
    [SerializeField, Tooltip("�T�C�Y�̊g�k�̕ω��̊ɋ}�i���C�[�W���O�j���w��")]
    private Ease scaleEase = Ease.Unset;

    /// <summary>
    /// �ő勗���e�L�X�g�̃T�C�Y�g�k�A�j���[�V����
    /// </summary>
    public void ScaleAnimation()
    {
        
        this.transform.localScale = Vector3.one;

        // �w�肵���l�̃T�C�Y��
        transform.DOScale( //�Ԃ�l��ۑ�    
            targetScale,                // �X�P�[���l
            scaleProductionTime         // ���o����
        )
        .SetLoops(2, scaleLoopType)     // �J��Ԃ��i2��j�A�ړ���A�ŏ��̈ʒu����Ăшړ����J�n
        .SetEase(scaleEase)             // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�   
        .SetLink(gameObject);           // ����GameObject���폜���ꂽ�����Ŏ~�߂�悤�ɕR�t����
    }

}
