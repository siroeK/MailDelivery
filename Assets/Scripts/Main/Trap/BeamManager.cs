using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �r�[���̊Ǘ��v���W�F�N�g
/// </summary>
public class BeamManager : MonoBehaviour
{
    [Header("�r�[���̃Q�[���I�[�o�[���R����")]
    [SerializeField, Tooltip("�r�[���̃Q�[���I�[�o�[���R����"), Multiline(2)]
    private string enemyExplanationText = "�d���p���X�ŒʐM�����f����A\n���[�����r�؂�Ă��܂����B";

    /// <summary>
    /// �r�[���̃Q�[���I�[�o�[���R����
    /// </summary>
    public string GetBeamExplanation()
    {
        return enemyExplanationText;
    }
}
