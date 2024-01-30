using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SE�T�E���h��1�̉����o���Ƃ��Ɏg���X�N���v�g
/// </summary>
public class SEButtonTap : MonoBehaviour
{

    [Header("�T�E���h�G�t�F�N�g")]
    [Tooltip("�{�^���^�b�v��SE")]
    [SerializeField] private AudioClip buttonTapSE;         // �{�^���^�b�v��SE

    // SE���Q�Ƃ�����
    private SESound SEManager;

    // Start is called before the first frame update
    void Start()
    {
        // BGM�ESE�̃X�N���v�g�Q��
        SEManager = GameObject.FindWithTag("SE").GetComponent<SESound>();
    }


    // �{�^��clickSE
    public void ButtonTapSE()
    {
        SEManager.SetSEOneShot(buttonTapSE);
    }

}
