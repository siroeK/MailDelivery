using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SE�T�E���h�̃X�N���v�g�A�V���O���g���ɂ��Ă���
/// </summary>
public class SESound : SingletonMonoBehaviour<SESound>
{

    // �I�u�W�F�N�g���V�[�����ς���Ă������Ȃ��ł�����
    protected override bool dontDestroyOnLoad { get { return true; } }

    private AudioSource SEAudioSource;                      // SE�̃I�[�f�B�I�\�[�X�������

    [Header("�T�E���h�G�t�F�N�g")]
    [Tooltip("����{�^����SE")]
    [SerializeField] private AudioClip decisionButtonSE;
    [Tooltip("�J�[�\���t���[���ړ���SE")]
    [SerializeField] private AudioClip cursorFrameMoveSE;
    [Tooltip("���{�^����SE")]
    [SerializeField] private AudioClip arrowButtonSE;
    [Tooltip("���[���~�߂�ǂ�SE")]
    [SerializeField] private AudioClip mailStopWallSE; 
    [Tooltip("���U���g�\����SE")]
    [SerializeField] private AudioClip resultDisplaySE;
    [Tooltip("��������SE")]
    [SerializeField] private AudioClip successSoundSE;
    [Tooltip("�v���C���[�ړ���SE")]
    [SerializeField] private AudioClip playerMoveSE;
    [Tooltip("�����ł̎��SSE")]
    [SerializeField] private AudioClip explosionDeathSE;
    [Tooltip("�r�[���ł̎��SSE")]
    [SerializeField] private AudioClip beamDeathSE;
    [Tooltip("�T�C�o�[�U���ł̎��SSE")]
    [SerializeField] private AudioClip cyberAttackDeathSE;

    // ������
    override protected void Awake()
    {
        base.Awake();   // �e�̂��(�V���O���g��)

        // �Q��
        SEAudioSource = GetComponent<AudioSource>();

        SEAudioSource.volume = 0.5f;
    }

    // SE�̉��̃{�����[����ς���
    public float GetSetSEVolume  //public �߂�l �v���p�e�B��
    {
        get { return SEAudioSource.volume; }  //get {return �t�B�[���h��;}
        set { SEAudioSource.volume = value; } //set {�t�B�[���h�� = value;}
    }

    // SE�̉��̃{�����[�����O�ɂ���
    public void SetSEVolumeZero()
    {
        SEAudioSource.volume = 0.0f;
    }

    // SE�ň�x�����炷����
    public void SetSEOneShot(AudioClip clip)
    {
        SEAudioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// ����{�^����SE
    /// </summary>
    public void DecisionButtonSE()
    {
        SEAudioSource.PlayOneShot(decisionButtonSE);
    }

    /// <summary>
    /// �J�[�\���t���[���ړ���SE
    /// </summary>
    public void CursorFrameMoveSE()
    {
        SEAudioSource.PlayOneShot(cursorFrameMoveSE);
    }
    
    /// <summary>
    /// ���{�^����SE
    /// </summary>
    public void ArrowButtonSE()
    {
        SEAudioSource.PlayOneShot(arrowButtonSE);
    }

    /// <summary>
    /// ���[���~�߂�ǂ�SE
    /// </summary>
    public void MailStopWallSE()
    {
        SEAudioSource.PlayOneShot(mailStopWallSE);
    }

    /// <summary>
    /// ���U���g�\����SE
    /// </summary>
    public void ResultDisplaySE()
    {
        SEAudioSource.PlayOneShot(resultDisplaySE);
    }

    /// <summary>
    /// ��������SE
    /// </summary>
    public void SuccessSoundSE()
    {
        SEAudioSource.PlayOneShot(successSoundSE);
    }

    /// <summary>
    /// �v���C���[�ړ���SE
    /// </summary>
    public void PlayerMoveSE()
    {
        SEAudioSource.PlayOneShot(playerMoveSE);
    }

    /// <summary>
    /// �����ł̎��SSE
    /// </summary>
    public void ExplosionDeathSE()
    {
        SEAudioSource.PlayOneShot(explosionDeathSE);
    }

    /// <summary>
    /// �r�[���ł̎��SSE
    /// </summary>
    public void BeamDeathSE()
    {
        SEAudioSource.PlayOneShot(beamDeathSE);
    }

    /// <summary>
    /// �T�C�o�[�U���ł̎��SSE
    /// </summary>
    public void CyberAttackDeathSE()
    {
        SEAudioSource.PlayOneShot(cyberAttackDeathSE);
    }
}
