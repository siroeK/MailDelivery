using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// BGM�T�E���h�̃X�N���v�g�A�V���O���g���ɂ��Ă���
/// </summary>
public class BGMSound : SingletonMonoBehaviour<BGMSound>
{

    // �I�u�W�F�N�g���V�[�����ς���Ă������Ȃ��ł�����
    protected override bool dontDestroyOnLoad { get { return true; } }

    private AudioSource BGMAudioSource;     // BGM�̃I�[�f�B�I�\�[�X�������


    [Header("�Q�[����BGM�̉�")]
    [Header("�^�C�g��BGM")]
    [SerializeField] private AudioClip titleBGM;


    // ������
    override protected void Awake()
    {
        base.Awake();   // �e�̂��(�V���O���g��)

        // �Q��
        BGMAudioSource = GetComponent<AudioSource>();

        BGMAudioSource.volume = 0.5f;

    }

    // BGM�̉��̃{�����[����ς���
    public float GetSetBGMVolume  //public �߂�l �v���p�e�B��
    {
        get { return BGMAudioSource.volume; }  //get {return �t�B�[���h��;}
        set { BGMAudioSource.volume = value; } //set {�t�B�[���h�� = value;}
    }

    // BGM�̉��̃{�����[�����O�ɂ���
    public void SetBGMVolumeZero()
    {
        BGMAudioSource.volume = 0.0f;
    }

    // BGM�������ւ��� 
    public void SetBGMClip(AudioClip clip)
    {
        BGMAudioSource.clip = clip;
        BGMAudioSource.Play();
    }

    // BGM�������Ă���I�u�W�F�N�g������
    public void BGMDestroy()
    {
        Destroy(this.gameObject);
    }

    // �^�C�g����BGM�ɕς���
    public void ChangeTitleBGM()
    {
        SetBGMClip(titleBGM);
    }


}
