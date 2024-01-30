using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/// <summary>
/// プレイヤーの管理プロジェクト
/// </summary>
public class PlayerManager : MonoBehaviour
{

    [SerializeField, Header("メインの管理のスクリプト参照")]
    private MainManager mainManager;

    [SerializeField, Header("ステージの情報のスクリプト参照")]
    private StageManager stageManager;

    [SerializeField, Header("プレイヤーのアニメーション参照")]
    private PlayerAnimation playerAnimation;

    [Header("移動した時の当たり判定")]
    [SerializeField, Tooltip("上の当たり判定")]
    private MoveHitDetection upHitDetection;
    [SerializeField, Tooltip("下の当たり判定")]
    private MoveHitDetection downHitDetection;
    [SerializeField, Tooltip("左の当たり判定")]
    private MoveHitDetection leftHitDetection;
    [SerializeField, Tooltip("右当たり判定")]
    private MoveHitDetection rightHitDetection;

    private PlayerInput playerInput;    // プレイヤーの入力

    private Rigidbody rigidBody;        // 質量

    [Header("移動させるアニメーション(DOLocalMoveX,Z)")]
    [SerializeField, Tooltip("移動の演出時間")]
    private float moveProductionTime = 1.0f;
    [SerializeField, Tooltip("移動の変化の緩急（＝イージング）を指定")]
    private Ease moveEase = Ease.Unset;

    [Header("指定した値に回転させる(DORotate)")]
    [SerializeField, Tooltip("移動の回転の演出時間")]
    private float rotateProductionTime = 1.0f;
    [SerializeField, Tooltip("回転の変化の緩急（＝イージング）を指定")]
    private Ease rotateEase = Ease.Unset;
    [SerializeField, Tooltip("ひっくり返る回転の演出時間")]
    private Vector3 deathRotate = new Vector3(-180f, 0f, 0f);
    [SerializeField, Tooltip("ひっくり返る回転の演出時間")]
    private float deathProductionTime = 1.0f;

    private Tween moveTween;            // 移動アニメーションの返り値を用意

    // SEを参照するやつ
    private SESound SESound;

    /// <summary>
    /// プレイヤーの移動状況
    /// </summary>
    private enum PlayerMoveState
    {
        Stop,   // 止まっている
        Up,     // 上に動く
        Down,   // 下に行く
        Right,  // 右に行く
        Left,   // 左に行く
    }

    private PlayerMoveState moveState = PlayerMoveState.Stop;


    private void Awake()
    {
        // Actionスクリプトのインスタンス生成
        playerInput = new PlayerInput();

        // Actionイベント登録
        playerInput.Player.Move.started += OnMove;      // 入力が0から0以外に変化したとき
        //playerInput.Player.Move.performed += OnMove;  // 入力が0以外に変化したとき
        //playerInput.Player.Move.canceled += OnMove;   // 入力が0以外から0に変化したとき

        // Input Actionを機能させるためには、有効化する必要がある
        playerInput.Enable();

        // 参照
        rigidBody = this.GetComponent<Rigidbody>();

        // SEのスクリプト参照
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        // プレイヤーの移動状況
        moveState = PlayerMoveState.Stop;
    }

    // プレイヤーの縦軸方向の移動(Z座標)
    private void PlayerVerticalMove(int move)
    {

        float playerPosZ = this.gameObject.transform.position.z + move;

        // 最背面の位置いかにはいけないように
        if (playerPosZ < stageManager.GetRearMostPos()) 
        {
            HitDetectionReset();
            moveState = PlayerMoveState.Stop; // 止まっている
            return;
        }

        VerticalAxisMove(playerPosZ);
    }

    // プレイヤーの横軸方向の移動(X座標)
    private void PlayerHorizontalMove(int move)
    {

        float playerPosX = this.gameObject.transform.position.x + move;

        // 最背面の位置いかにはいけないように
        if (playerPosX > stageManager.GetMaxSidePos() || playerPosX < -stageManager.GetMaxSidePos())
        {
            HitDetectionReset();
            moveState = PlayerMoveState.Stop; // 止まっている
            return;
        }

        HorizontalAxisMove(playerPosX);
    }

    /// <summary>
    /// DOTweenを使った縦軸方向の移動アニメーション
    /// </summary>
    private void VerticalAxisMove(float targetZ)
    {
        // ローカル座標で移動
        this.moveTween = transform.DOLocalMoveZ(
            targetZ,                    // 移動終了地点
            moveProductionTime          // 演出時間
        )
        .SetEase(moveEase)              // 変化の緩急（＝イージング）を指定、一定の速度で値が変化
        .OnStart(() =>                  // 実行開始時のコールバック
        {
            playerAnimation.MoveAnimation();    // 移動時アニメーション
        })
        .OnComplete(() =>               // 実行完了時のコールバック
        {
            HitDetectionReset();
            moveState = PlayerMoveState.Stop; // 止まっている
            mainManager.SetMaxAdvanced(this.gameObject.transform.position.z);   // プレイヤーの最大の進んだ距離か確認
            playerAnimation.MoveStopAnimation();    // 移動時アニメーションを止める
        })
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける
    }

    /// <summary>
    /// DOTweenを使った横軸方向の移動アニメーション
    /// </summary>
    private void HorizontalAxisMove(float targetX)
    {
        // ローカル座標で移動
        this.moveTween = transform.DOLocalMoveX(
            targetX,                    // 移動終了地点
            moveProductionTime          // 演出時間
        )
        .SetEase(moveEase)              // 変化の緩急（＝イージング）を指定、一定の速度で値が変化
        .OnStart(() =>                  // 実行開始時のコールバック
        {
            playerAnimation.MoveAnimation();    // 移動時アニメーション
        })
        .OnComplete(() =>               // 実行完了時のコールバック
        {
            HitDetectionReset();
            moveState = PlayerMoveState.Stop; // 止まっている
            playerAnimation.MoveStopAnimation();    // 移動時アニメーションを止める
        })
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける
    }

    // 障害物があるかどうかをリセット
    private void HitDetectionReset()
    {
        upHitDetection.MoveHitDetectionReset();
        downHitDetection.MoveHitDetectionReset();
        leftHitDetection.MoveHitDetectionReset();
        rightHitDetection.MoveHitDetectionReset();
    }

    private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        playerInput?.Dispose();
    }

    /// <summary>
    /// 入力取得での移動
    /// </summary>
    private void OnMove(InputAction.CallbackContext context)
    {
        // ゲームプレイ状況が「プレイ中」の時以外は返す
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.DuringPlay) return;

        // 止まっている以外は返す
        if (moveState != PlayerMoveState.Stop) return;

        Vector2 inputValue = context.ReadValue<Vector2>();  // アクションの入力取得

        // 入力状況によって移動状況を変更する
        if (inputValue.x != 0)
        {
            // 横軸方向への移動
            if (InputValueLeftRight((int)inputValue.x))
            {
                PlayerHorizontalMove((int)inputValue.x);
            }
            // 移動しない
            else
            {
                // 障害物又は想定外のエラー時
                HitDetectionReset();
                moveState = PlayerMoveState.Stop; // 止まっている
            }
        }
        else if (inputValue.y != 0)
        {
            // 縦軸方向への移動
            if (InputValueUpDown((int)inputValue.y))
            {
                PlayerVerticalMove((int)inputValue.y);
            }
            // 移動しない
            else
            {
                // 障害物又は想定外のエラー時
                HitDetectionReset();
                moveState = PlayerMoveState.Stop; // 止まっている
            }
        }
    }

    // 入力された値が上下の時
    private bool InputValueUpDown(int value)
    {
        // プレイヤーの移動状況を変更
        switch (value)
        {
            // 上
            case 1:
                moveState = PlayerMoveState.Up; // 上に動く
                CharactertRotateAnimation(new Vector3(-90f, 0f, 0f), rotateProductionTime);

                // 上に障害物があった時
                if(upHitDetection.GetMoveHitDetection()) return false;

                break; 

            // 下
            case -1:
                moveState = PlayerMoveState.Down; // 下に動く
                CharactertRotateAnimation(new Vector3(-90f, 180f, 0f), rotateProductionTime);

                // 下に障害物があった時
                if (downHitDetection.GetMoveHitDetection()) return false;
                break;

            default:
                Debug.LogError($"想定外の上下入力値エラー:" + value);
                return false;
        }

        return true;
    }

    // 入力された値が左右の時
    private bool InputValueLeftRight(int value)
    {
        // プレイヤーの移動状況を変更
        switch (value)
        {
            // 右
            case 1:
                moveState = PlayerMoveState.Right; // 右に動く
                CharactertRotateAnimation(new Vector3(-90f, 90f, 0f), rotateProductionTime);

                // 右に障害物があった時
                if (rightHitDetection.GetMoveHitDetection()) return false;
                break;

            // 左
            case -1:
                moveState = PlayerMoveState.Left; // 左に動く
                CharactertRotateAnimation(new Vector3(-90f, 270f, 0f), rotateProductionTime);

                // 左に障害物があった時
                if (leftHitDetection.GetMoveHitDetection()) return false;
                break;

            default:
                Debug.LogError($"想定外の左右入力値エラー:" + value);
                return false;
        }

        return true;

    }

    /// <summary>
    /// キャラクターの回転アニメーション
    /// </summary>
    public void CharactertRotateAnimation(Vector3 rotate, float time)
    {
        // 回転
        transform.DORotate(
            rotate,                     // 終了時のRotation
            time                        // 演出時間
        )
        .SetEase(rotateEase)            // 変化の緩急（＝イージング）を指定、一定の速度で値が変化
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける
    }

    // ゲームオブジェクト同士が接触したタイミングで実行
    private void OnTriggerEnter(Collider other)
    {
        if (mainManager.GetGamePlayStatus() == MainManager.GamePlayStatus.Result) return;

        // 敵と当たった時
        if (other.gameObject.CompareTag("Enemy"))
        {
            this.moveTween.Kill();      // 移動を止める

            // 座標固定
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, other.transform.position.z);

            // ゲームプレイ状況を「結果」にする
            mainManager.SetGamePlayStatus(MainManager.GamePlayStatus.Result);

            ViralEnemyManager enemy = other.GetComponent<ViralEnemyManager>();
            enemy.ExplosionEffectGeneration();  // 爆発エフェクト生成
            mainManager.GameOverResultDisplay(enemy.GetEnemyExplanation());    // ゲームオーバー結果を表示
            Destroy(enemy.gameObject); // 削除

            // 爆発での死亡SE
            SESound.ExplosionDeathSE();

            // キャラクターをひっくり返す
            CharactertRotateAnimation(deathRotate, deathProductionTime);
        }

        // ゴール時
        else if (other.CompareTag("GoalLineArea"))
        {
            // ゲームプレイ状況を「結果」にする
            mainManager.SetGamePlayStatus(MainManager.GamePlayStatus.Result);
            mainManager.GameGoalResultDisplay();    // ゴール結果を表示
        }

        // ビームと当たった時
        else if (other.gameObject.CompareTag("Beam"))
        {
            this.moveTween.Kill();      // 移動を止める

            // 座標固定
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, other.transform.position.z);

            // ゲームプレイ状況を「結果」にする
            mainManager.SetGamePlayStatus(MainManager.GamePlayStatus.Result);

            BeamManager beam = other.GetComponent<BeamManager>();
            mainManager.GameOverResultDisplay(beam.GetBeamExplanation());    // ゲームオーバー結果を表示

            //ビームでの死亡SE
            SESound.BeamDeathSE();

            // キャラクターをひっくり返す
            CharactertRotateAnimation(deathRotate, deathProductionTime);
        }

        // サイバー攻撃と当たった時
        else if (other.gameObject.CompareTag("CyberAttack"))
        {
            this.moveTween.Kill();      // 移動を止める

            // 座標固定
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, other.transform.position.z);

            // ゲームプレイ状況を「結果」にする
            mainManager.SetGamePlayStatus(MainManager.GamePlayStatus.Result);

            CyberAttackManager cyberAttack = other.GetComponent<CyberAttackManager>();
            mainManager.GameOverResultDisplay(cyberAttack.GetBeamExplanation());    // ゲームオーバー結果を表示

            // サイバー攻撃での死亡SE
            SESound.CyberAttackDeathSE();

            // キャラクターをひっくり返す
            CharactertRotateAnimation(deathRotate, deathProductionTime);
        }

    }

    /// <summary>
    /// キャラクターの移動状況を返す
    /// </summary>
    public bool PlayerMoveStopCheck()
    {
        if(moveState == PlayerMoveState.Stop) return true;

        return false;
    }

}
