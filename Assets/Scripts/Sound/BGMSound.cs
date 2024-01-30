using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// BGMサウンドのスクリプト、シングルトンにしておく
/// </summary>
public class BGMSound : SingletonMonoBehaviour<BGMSound>
{

    // オブジェクトがシーンが変わっても消さないでおくか
    protected override bool dontDestroyOnLoad { get { return true; } }

    private AudioSource BGMAudioSource;     // BGMのオーディオソース入れるやつ


    [Header("ゲームのBGMの音")]
    [Header("タイトルBGM")]
    [SerializeField] private AudioClip titleBGM;


    // 初期化
    override protected void Awake()
    {
        base.Awake();   // 親のやつ(シングルトン)

        // 参照
        BGMAudioSource = GetComponent<AudioSource>();

        BGMAudioSource.volume = 0.5f;

    }

    // BGMの音のボリュームを変える
    public float GetSetBGMVolume  //public 戻り値 プロパティ名
    {
        get { return BGMAudioSource.volume; }  //get {return フィールド名;}
        set { BGMAudioSource.volume = value; } //set {フィールド名 = value;}
    }

    // BGMの音のボリュームを０にする
    public void SetBGMVolumeZero()
    {
        BGMAudioSource.volume = 0.0f;
    }

    // BGMを差し替える 
    public void SetBGMClip(AudioClip clip)
    {
        BGMAudioSource.clip = clip;
        BGMAudioSource.Play();
    }

    // BGMが入っているオブジェクトを消す
    public void BGMDestroy()
    {
        Destroy(this.gameObject);
    }

    // タイトルのBGMに変える
    public void ChangeTitleBGM()
    {
        SetBGMClip(titleBGM);
    }


}
