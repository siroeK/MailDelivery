using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using unityroom.Api;

/// <summary>
/// メインの管理プロジェクト
/// </summary>
public class MainManager : MonoBehaviour
{
    [SerializeField, Header("プレイヤーのオブジェクト参照")]
    private Transform player;

    [SerializeField, Header("メールを止める壁を出すオブジェクト参照")]
    private MailStopWall mailStopWall;

    [SerializeField, Header("ゲーム結果のオブジェクト参照")]
    private GameResult gameResult;

    [SerializeField, Header("最大の進んだ距離テキスト参照")]
    private TextMeshProUGUI maxDistanceText;

    [SerializeField, Header("最大の進んだ距離アニメーション参照")]
    private MaxDistanceAnimation maxDistanceAnimation;

    private int maxAdvancedDistance = 0;      // 最大の進んだ距離

    private float maxPlayerAdvancedDistance = 0;    // プレイヤーの最大の進んだ距離    

    // SEを参照するやつ
    private SESound SESound;

    /// <summary>
    /// ゲームプレイ状況
    /// </summary>
    public enum GamePlayStatus
    {
        Start,      // スタート
        DuringPlay, // プレイ中
        Result,     // 結果
    }
    private GamePlayStatus gamePlayStatus = GamePlayStatus.Start;


    private void Awake()
    {
        maxPlayerAdvancedDistance = player.transform.position.z;

        maxDistanceText.text = maxAdvancedDistance.ToString();  // 最大の進んだ距離テキスト表示

        gamePlayStatus = GamePlayStatus.DuringPlay;

        // SEのスクリプト参照
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();
    }

    /// <summary>
    /// 最大の進んだを入れる(前後移動したごとに確認)
    /// </summary>
    public void SetMaxAdvanced(float posZ)
    {
        // 最大の進んだ距離
        if (posZ > maxPlayerAdvancedDistance)
        {
            maxPlayerAdvancedDistance = posZ;   // プレイヤーの最大の進んだ距離
            maxAdvancedDistance++;              // 最大の進んだ距離
            mailStopWall.MailStopTimeReset();   // 時間のリセット

            maxDistanceAnimation.ScaleAnimation();
            maxDistanceText.text = maxAdvancedDistance.ToString();  // 最大の進んだ距離テキスト表示を更新
        }
    }

    /// <summary>
    /// ゲームプレイ状況を渡す
    /// </summary>
    public GamePlayStatus GetGamePlayStatus()
    {
        return gamePlayStatus;
    }

    /// <summary>
    /// ゲームプレイ状況を入れる
    /// </summary>
    public void SetGamePlayStatus(GamePlayStatus Status)
    {
        gamePlayStatus = Status;
    }

    /// <summary>
    /// ゴールした時の結果を表示
    /// </summary>
    public void GameGoalResultDisplay()
    {
        // 成功音のSE
        SESound.SuccessSoundSE();

        gameResult.GameGoalResult();
    }

    /// <summary>
    /// ゲームオーバー時の結果を表示
    /// </summary>
    public void GameOverResultDisplay(string explanation)
    {
        // 無限モードの時
        if (SceneManager.GetActiveScene().name == "EndlessMain")
        {
            // ゲームの最大の進行距離を更新しているかどうか
            if (TitleManager.gameMaxAdvanced <= maxAdvancedDistance)
            {
                TitleManager.gameMaxAdvanced = maxAdvancedDistance; // 更新する
            }

            // unityroomのボードNo1に最大の進行距離を送信する。
            UnityroomApiClient.Instance.SendScore(1, TitleManager.gameMaxAdvanced, ScoreboardWriteMode.HighScoreDesc);

            gameResult.EndlessGameOverResult(explanation, maxAdvancedDistance); // リザルト表示
        }
        // それ以外
        else
        {
            gameResult.GameOverResult(explanation); // リザルト表示
        }
    }

}
