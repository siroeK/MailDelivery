using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SEサウンドのスクリプト、シングルトンにしておく
/// </summary>
public class SESound : SingletonMonoBehaviour<SESound>
{

    // オブジェクトがシーンが変わっても消さないでおくか
    protected override bool dontDestroyOnLoad { get { return true; } }

    private AudioSource SEAudioSource;                      // SEのオーディオソース入れるやつ

    [Header("サウンドエフェクト")]
    [Tooltip("決定ボタンのSE")]
    [SerializeField] private AudioClip decisionButtonSE;
    [Tooltip("カーソルフレーム移動のSE")]
    [SerializeField] private AudioClip cursorFrameMoveSE;
    [Tooltip("矢印ボタンのSE")]
    [SerializeField] private AudioClip arrowButtonSE;
    [Tooltip("メール止める壁のSE")]
    [SerializeField] private AudioClip mailStopWallSE; 
    [Tooltip("リザルト表示のSE")]
    [SerializeField] private AudioClip resultDisplaySE;
    [Tooltip("成功音のSE")]
    [SerializeField] private AudioClip successSoundSE;
    [Tooltip("プレイヤー移動のSE")]
    [SerializeField] private AudioClip playerMoveSE;
    [Tooltip("爆発での死亡SE")]
    [SerializeField] private AudioClip explosionDeathSE;
    [Tooltip("ビームでの死亡SE")]
    [SerializeField] private AudioClip beamDeathSE;
    [Tooltip("サイバー攻撃での死亡SE")]
    [SerializeField] private AudioClip cyberAttackDeathSE;

    // 初期化
    override protected void Awake()
    {
        base.Awake();   // 親のやつ(シングルトン)

        // 参照
        SEAudioSource = GetComponent<AudioSource>();

        SEAudioSource.volume = 0.5f;
    }

    // SEの音のボリュームを変える
    public float GetSetSEVolume  //public 戻り値 プロパティ名
    {
        get { return SEAudioSource.volume; }  //get {return フィールド名;}
        set { SEAudioSource.volume = value; } //set {フィールド名 = value;}
    }

    // SEの音のボリュームを０にする
    public void SetSEVolumeZero()
    {
        SEAudioSource.volume = 0.0f;
    }

    // SEで一度だけ鳴らすもの
    public void SetSEOneShot(AudioClip clip)
    {
        SEAudioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// 決定ボタンのSE
    /// </summary>
    public void DecisionButtonSE()
    {
        SEAudioSource.PlayOneShot(decisionButtonSE);
    }

    /// <summary>
    /// カーソルフレーム移動のSE
    /// </summary>
    public void CursorFrameMoveSE()
    {
        SEAudioSource.PlayOneShot(cursorFrameMoveSE);
    }
    
    /// <summary>
    /// 矢印ボタンのSE
    /// </summary>
    public void ArrowButtonSE()
    {
        SEAudioSource.PlayOneShot(arrowButtonSE);
    }

    /// <summary>
    /// メール止める壁のSE
    /// </summary>
    public void MailStopWallSE()
    {
        SEAudioSource.PlayOneShot(mailStopWallSE);
    }

    /// <summary>
    /// リザルト表示のSE
    /// </summary>
    public void ResultDisplaySE()
    {
        SEAudioSource.PlayOneShot(resultDisplaySE);
    }

    /// <summary>
    /// 成功音のSE
    /// </summary>
    public void SuccessSoundSE()
    {
        SEAudioSource.PlayOneShot(successSoundSE);
    }

    /// <summary>
    /// プレイヤー移動のSE
    /// </summary>
    public void PlayerMoveSE()
    {
        SEAudioSource.PlayOneShot(playerMoveSE);
    }

    /// <summary>
    /// 爆発での死亡SE
    /// </summary>
    public void ExplosionDeathSE()
    {
        SEAudioSource.PlayOneShot(explosionDeathSE);
    }

    /// <summary>
    /// ビームでの死亡SE
    /// </summary>
    public void BeamDeathSE()
    {
        SEAudioSource.PlayOneShot(beamDeathSE);
    }

    /// <summary>
    /// サイバー攻撃での死亡SE
    /// </summary>
    public void CyberAttackDeathSE()
    {
        SEAudioSource.PlayOneShot(cyberAttackDeathSE);
    }
}
