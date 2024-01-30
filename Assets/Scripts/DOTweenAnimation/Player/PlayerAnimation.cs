using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// プレイヤーのアニメーション
/// </summary>
public class PlayerAnimation : MonoBehaviour
{

    [Header("オブジェクト")]
    [SerializeField, Tooltip("胴体部分のオブジェクト")]
    private Transform body = null;
    [SerializeField, Tooltip("左足部分のオブジェクト")]
    private Transform leftFoot = null;
    [SerializeField, Tooltip("右足部分のオブジェクト")]
    private Transform rightFoot = null;

    [Header("胴体分のアニメーション(DOLocalMoveZ)")]
    [SerializeField, Tooltip("胴体の目標座標")]
    private float bodyTargetPosZ = 0.15f;
    [SerializeField, Tooltip("演出時間")]
    private float bodyProductionTime = 0.75f;
    [SerializeField, Tooltip("繰り返しタイプ")]
    private LoopType bodyLoopType = LoopType.Restart;
    [SerializeField, Tooltip("変化の緩急（＝イージング）を指定")]
    private Ease bodyEase = Ease.Unset;

    [Header("移動のアニメーション")]
    [Header("設定した複数の通過地点を通って移動させる(DOLocalPath)")]
    [SerializeField, Tooltip("右足移動の経由地点")]
    private Vector3[] rightMoveWayPoints = new Vector3[2];
    [SerializeField, Tooltip("左足移動の経由地点")]
    private Vector3[] leftMoveWayPoints = new Vector3[2];
    [SerializeField, Tooltip("移動の演出時間")]
    private float moveProductionTime = 0.35f;
    [SerializeField, Tooltip("移動の曲線の種類")]
    private PathType movePathType = PathType.Linear;
    [SerializeField, Tooltip("移動の「最後のWayPoint→最初のWayPoint」が繋がるようするか")]
    private bool moveOption = true;
    [SerializeField, Tooltip("移動の繰り返しタイプ")]
    private LoopType moveLoopType = LoopType.Restart;
    [SerializeField, Tooltip("移動の変化の緩急（＝イージング）を指定")]
    private Ease moveEase = Ease.Unset;

    [SerializeField, Tooltip("止まる時の左足の座標")]
    private Vector3 leftFootPos = Vector3.zero;
    [SerializeField, Tooltip("止まる時の右足の座標")]
    private Vector3 rightFootPos = Vector3.zero;

    private Tween rightMoveTween;       // 右足の移動時アニメーションの返り値を用意
    private Tween leftMoveTween;        // 左足の移動時アニメーションの返り値を用意

    // SEを参照するやつ
    private SESound SESound;

    // Start is called before the first frame update
    void Start()
    {
        // SEのスクリプト参照
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        // アニメーション
        BodyAnimation();
    }

    /// <summary>
    /// DOTweenを使った胴体のアニメーション
    /// </summary>
    private void BodyAnimation()
    {
        // ローカル座標でZ移動
        body.DOLocalMoveZ(
            bodyTargetPosZ,             // 移動終了地点
            bodyProductionTime          // 演出時間
        )
        .SetLoops(-1, bodyLoopType)     // 繰り返し（-1 = ループ）、移動後、最初の位置から再び移動を開始
        .SetEase(bodyEase)              // 変化の緩急（＝イージング）を指定、一定の速度で値が変化
        .OnStart(() =>                  // 実行開始時のコールバック
        {

        })
        .OnComplete(() =>               // 実行完了時のコールバック
        {

        })
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける
    }

    /// <summary>
    /// DOTweenを使った移動時アニメーション
    /// </summary>
    public void MoveAnimation()
    {

        // メール止める壁のSE
        SESound.PlayerMoveSE();

        //  ローカル座標で移動(左足)
        leftMoveTween = leftFoot.transform.DOLocalPath(
            leftMoveWayPoints,          // 通過地点のリスト
            moveProductionTime,         // 演出時間
            movePathType                // 曲線の種類 、Catmull-Rom曲線を使用(WayPoint同士を曲線で繋ぐ)
                                        //gizmoColor: Color.red     // Gizmosで赤色を設定
        )
        .SetOptions(moveOption)         // 「最後のWayPoint→最初のWayPoint」が繋がるようになります
        .SetLoops(-1, moveLoopType)     // 繰り返し（ループ）、移動後、最初の位置から再び移動を開始
        .SetEase(moveEase)              // 変化の緩急（＝イージング）を指定、一定の速度で値が変化
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける

        //  ローカル座標で移動(右足)
        rightMoveTween = rightFoot.transform.DOLocalPath(
            rightMoveWayPoints,         // 通過地点のリスト
            moveProductionTime,             // 演出時間
            movePathType                // 曲線の種類 、Catmull-Rom曲線を使用(WayPoint同士を曲線で繋ぐ)
                                        //gizmoColor: Color.red     // Gizmosで赤色を設定
        )
        .SetOptions(moveOption)         // 「最後のWayPoint→最初のWayPoint」が繋がるようになります
        .SetLoops(-1, moveLoopType)     // 繰り返し（ループ）、移動後、最初の位置から再び移動を開始
        .SetEase(moveEase)              // 変化の緩急（＝イージング）を指定、一定の速度で値が変化
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける
    }

    /// <summary>
    /// 移動アニメーションを止める
    /// </summary>
    public void MoveStopAnimation()
    {
        // 移動時アニメーションを止める
        this.leftMoveTween.Kill();
        this.rightMoveTween.Kill();

        // 止まる時の左足、右足の座標を修正
        leftFoot.localPosition = leftFootPos;
        rightFoot.localPosition = rightFootPos;
    }

}
