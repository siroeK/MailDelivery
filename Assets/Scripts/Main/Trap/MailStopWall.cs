using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メールを止める壁を出すプロジェクト
/// </summary>
public class MailStopWall : MonoBehaviour
{

    [SerializeField, Header("メインの管理のスクリプト参照")]
    private MainManager mainManager;

    [SerializeField, Header("プレイヤーのオブジェクト参照")]
    private PlayerManager player;

    [Header("メールを止める壁の演出を出す時間")]
    [SerializeField, Tooltip("メールを止める壁の演出時間")]
    private float mailStopTime = 10f;

    private float stopTime = 0f;

    [Header("止める演出理由文章")]
    [SerializeField, Tooltip("止める演出理由文章"), Multiline(2)]
    private string stopExplanationText = "時間の経過によるエラーで、\nメールが失敗しました。";

    [Header("移動させるアニメーション(DOLocalMove)")]
    [SerializeField, Tooltip("移動の演出時間")]
    private float moveProductionTime = 0.5f;
    [SerializeField, Tooltip("移動の変化の緩急（＝イージング）を指定")]
    private Ease moveEase = Ease.Unset;

    // SEを参照するやつ
    private SESound SESound;

    // Start is called before the first frame update
    void Start()
    {
        // SEのスクリプト参照
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        MailStopTimeReset();
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームプレイ状況が「プレイ中」の時以外は返す
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.DuringPlay) return;

        if (stopTime < 0f) 
        {
            // プレイヤーが止まっているか確認
            if(player.PlayerMoveStopCheck())
            {
                // 座標をプレイヤーの1つ前に
                this.transform.position = new Vector3(0f, -0.1f, player.gameObject.transform.position.z + 1);

                // ゲームプレイ状況を「結果」にする
                mainManager.SetGamePlayStatus(MainManager.GamePlayStatus.Result);

                // 壁を出す
                VerticalAxisMove();
            }
        }
        else
        {
            stopTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// メールを止める壁の演出を出す時間のリセット
    /// </summary>
    public void MailStopTimeReset()
    {
        stopTime = mailStopTime;
    }

    /// <summary>
    /// DOTweenを使った縦軸方向の移動アニメーション
    /// </summary>
    private void VerticalAxisMove()
    {
        // ローカル座標で移動
        transform.DOLocalMoveY(
            2.0f,                       // 移動終了地点
            moveProductionTime          // 演出時間
        )
        .SetEase(moveEase)              // 変化の緩急（＝イージング）を指定、一定の速度で値が変化
        .OnStart(() =>                  // 実行開始時のコールバック
        {
            // メール止める壁のSE
            SESound.MailStopWallSE();
        })
        .OnComplete(() =>               // 実行完了時のコールバック
        {
            mainManager.GameOverResultDisplay(stopExplanationText);    // ゲームオーバー結果を表示
        });
    }

}
