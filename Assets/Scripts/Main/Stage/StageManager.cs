using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�W�̊Ǘ��v���W�F�N�g
/// </summary>
public class StageManager : MonoBehaviour
{
    [Header("�Q�[���X�e�[�W�̏��")]
    [SerializeField, Tooltip("�Ŕw�ʂ̈ʒu")]
    private float rearMostPos = -0.5f;
    [SerializeField, Tooltip("�ő呤�ʂ̈ʒu")]
    private float maxSidePos = 4.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �Ŕw�ʂ̈ʒu����n��
    /// </summary>
    public float GetRearMostPos()
    {
        return rearMostPos;
    }

    /// <summary>
    /// �ő呤�ʂ̈ʒu����n��
    /// </summary>
    public float GetMaxSidePos()
    {
        return maxSidePos;
    }


}
