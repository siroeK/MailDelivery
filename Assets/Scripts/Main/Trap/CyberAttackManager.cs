using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �T�C�o�[�U���̊Ǘ��v���W�F�N�g
/// </summary>
public class CyberAttackManager : MonoBehaviour
{
    [Header("�T�C�o�[�U���̃Q�[���I�[�o�[���R����")]
    [SerializeField, Tooltip("�T�C�o�[�U���̃Q�[���I�[�o�[���R����"), Multiline(2)]
    private string enemyExplanationText = "�T�C�o�[�U�����������A\n�s���A�N�Z�X���󂯂܂����B";

    /// <summary>
    /// �T�C�o�[�U���̃Q�[���I�[�o�[���R����
    /// </summary>
    public string GetBeamExplanation()
    {
        return enemyExplanationText;
    }
}
