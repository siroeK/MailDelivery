using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/// <summary>
/// �v���C���[�̊Ǘ��v���W�F�N�g
/// </summary>
public class PlayerManager : MonoBehaviour
{

    [SerializeField, Header("���C���̊Ǘ��̃X�N���v�g�Q��")]
    private MainManager mainManager;

    [SerializeField, Header("�X�e�[�W�̏��̃X�N���v�g�Q��")]
    private StageManager stageManager;

    [SerializeField, Header("�v���C���[�̃A�j���[�V�����Q��")]
    private PlayerAnimation playerAnimation;

    [Header("�ړ��������̓����蔻��")]
    [SerializeField, Tooltip("��̓����蔻��")]
    private MoveHitDetection upHitDetection;
    [SerializeField, Tooltip("���̓����蔻��")]
    private MoveHitDetection downHitDetection;
    [SerializeField, Tooltip("���̓����蔻��")]
    private MoveHitDetection leftHitDetection;
    [SerializeField, Tooltip("�E�����蔻��")]
    private MoveHitDetection rightHitDetection;

    private PlayerInput playerInput;    // �v���C���[�̓���

    private Rigidbody rigidBody;        // ����

    [Header("�ړ�������A�j���[�V����(DOLocalMoveX,Z)")]
    [SerializeField, Tooltip("�ړ��̉��o����")]
    private float moveProductionTime = 1.0f;
    [SerializeField, Tooltip("�ړ��̕ω��̊ɋ}�i���C�[�W���O�j���w��")]
    private Ease moveEase = Ease.Unset;

    [Header("�w�肵���l�ɉ�]������(DORotate)")]
    [SerializeField, Tooltip("�ړ��̉�]�̉��o����")]
    private float rotateProductionTime = 1.0f;
    [SerializeField, Tooltip("��]�̕ω��̊ɋ}�i���C�[�W���O�j���w��")]
    private Ease rotateEase = Ease.Unset;
    [SerializeField, Tooltip("�Ђ�����Ԃ��]�̉��o����")]
    private Vector3 deathRotate = new Vector3(-180f, 0f, 0f);
    [SerializeField, Tooltip("�Ђ�����Ԃ��]�̉��o����")]
    private float deathProductionTime = 1.0f;

    private Tween moveTween;            // �ړ��A�j���[�V�����̕Ԃ�l��p��

    // SE���Q�Ƃ�����
    private SESound SESound;

    /// <summary>
    /// �v���C���[�̈ړ���
    /// </summary>
    private enum PlayerMoveState
    {
        Stop,   // �~�܂��Ă���
        Up,     // ��ɓ���
        Down,   // ���ɍs��
        Right,  // �E�ɍs��
        Left,   // ���ɍs��
    }

    private PlayerMoveState moveState = PlayerMoveState.Stop;


    private void Awake()
    {
        // Action�X�N���v�g�̃C���X�^���X����
        playerInput = new PlayerInput();

        // Action�C�x���g�o�^
        playerInput.Player.Move.started += OnMove;      // ���͂�0����0�ȊO�ɕω������Ƃ�
        //playerInput.Player.Move.performed += OnMove;  // ���͂�0�ȊO�ɕω������Ƃ�
        //playerInput.Player.Move.canceled += OnMove;   // ���͂�0�ȊO����0�ɕω������Ƃ�

        // Input Action���@�\�����邽�߂ɂ́A�L��������K�v������
        playerInput.Enable();

        // �Q��
        rigidBody = this.GetComponent<Rigidbody>();

        // SE�̃X�N���v�g�Q��
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        // �v���C���[�̈ړ���
        moveState = PlayerMoveState.Stop;
    }

    // �v���C���[�̏c�������̈ړ�(Z���W)
    private void PlayerVerticalMove(int move)
    {

        float playerPosZ = this.gameObject.transform.position.z + move;

        // �Ŕw�ʂ̈ʒu�����ɂ͂����Ȃ��悤��
        if (playerPosZ < stageManager.GetRearMostPos()) 
        {
            HitDetectionReset();
            moveState = PlayerMoveState.Stop; // �~�܂��Ă���
            return;
        }

        VerticalAxisMove(playerPosZ);
    }

    // �v���C���[�̉��������̈ړ�(X���W)
    private void PlayerHorizontalMove(int move)
    {

        float playerPosX = this.gameObject.transform.position.x + move;

        // �Ŕw�ʂ̈ʒu�����ɂ͂����Ȃ��悤��
        if (playerPosX > stageManager.GetMaxSidePos() || playerPosX < -stageManager.GetMaxSidePos())
        {
            HitDetectionReset();
            moveState = PlayerMoveState.Stop; // �~�܂��Ă���
            return;
        }

        HorizontalAxisMove(playerPosX);
    }

    /// <summary>
    /// DOTween���g�����c�������̈ړ��A�j���[�V����
    /// </summary>
    private void VerticalAxisMove(float targetZ)
    {
        // ���[�J�����W�ňړ�
        this.moveTween = transform.DOLocalMoveZ(
            targetZ,                    // �ړ��I���n�_
            moveProductionTime          // ���o����
        )
        .SetEase(moveEase)              // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .OnStart(() =>                  // ���s�J�n���̃R�[���o�b�N
        {
            playerAnimation.MoveAnimation();    // �ړ����A�j���[�V����
        })
        .OnComplete(() =>               // ���s�������̃R�[���o�b�N
        {
            HitDetectionReset();
            moveState = PlayerMoveState.Stop; // �~�܂��Ă���
            mainManager.SetMaxAdvanced(this.gameObject.transform.position.z);   // �v���C���[�̍ő�̐i�񂾋������m�F
            playerAnimation.MoveStopAnimation();    // �ړ����A�j���[�V�������~�߂�
        })
        .SetLink(gameObject);           // ����GameObject���폜���ꂽ�����Ŏ~�߂�悤�ɕR�t����
    }

    /// <summary>
    /// DOTween���g�������������̈ړ��A�j���[�V����
    /// </summary>
    private void HorizontalAxisMove(float targetX)
    {
        // ���[�J�����W�ňړ�
        this.moveTween = transform.DOLocalMoveX(
            targetX,                    // �ړ��I���n�_
            moveProductionTime          // ���o����
        )
        .SetEase(moveEase)              // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .OnStart(() =>                  // ���s�J�n���̃R�[���o�b�N
        {
            playerAnimation.MoveAnimation();    // �ړ����A�j���[�V����
        })
        .OnComplete(() =>               // ���s�������̃R�[���o�b�N
        {
            HitDetectionReset();
            moveState = PlayerMoveState.Stop; // �~�܂��Ă���
            playerAnimation.MoveStopAnimation();    // �ړ����A�j���[�V�������~�߂�
        })
        .SetLink(gameObject);           // ����GameObject���폜���ꂽ�����Ŏ~�߂�悤�ɕR�t����
    }

    // ��Q�������邩�ǂ��������Z�b�g
    private void HitDetectionReset()
    {
        upHitDetection.MoveHitDetectionReset();
        downHitDetection.MoveHitDetectionReset();
        leftHitDetection.MoveHitDetectionReset();
        rightHitDetection.MoveHitDetectionReset();
    }

    private void OnDestroy()
    {
        // ���g�ŃC���X�^���X������Action�N���X��IDisposable���������Ă���̂ŁA
        // �K��Dispose����K�v������
        playerInput?.Dispose();
    }

    /// <summary>
    /// ���͎擾�ł̈ړ�
    /// </summary>
    private void OnMove(InputAction.CallbackContext context)
    {
        // �Q�[���v���C�󋵂��u�v���C���v�̎��ȊO�͕Ԃ�
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.DuringPlay) return;

        // �~�܂��Ă���ȊO�͕Ԃ�
        if (moveState != PlayerMoveState.Stop) return;

        Vector2 inputValue = context.ReadValue<Vector2>();  // �A�N�V�����̓��͎擾

        // ���͏󋵂ɂ���Ĉړ��󋵂�ύX����
        if (inputValue.x != 0)
        {
            // ���������ւ̈ړ�
            if (InputValueLeftRight((int)inputValue.x))
            {
                PlayerHorizontalMove((int)inputValue.x);
            }
            // �ړ����Ȃ�
            else
            {
                // ��Q�����͑z��O�̃G���[��
                HitDetectionReset();
                moveState = PlayerMoveState.Stop; // �~�܂��Ă���
            }
        }
        else if (inputValue.y != 0)
        {
            // �c�������ւ̈ړ�
            if (InputValueUpDown((int)inputValue.y))
            {
                PlayerVerticalMove((int)inputValue.y);
            }
            // �ړ����Ȃ�
            else
            {
                // ��Q�����͑z��O�̃G���[��
                HitDetectionReset();
                moveState = PlayerMoveState.Stop; // �~�܂��Ă���
            }
        }
    }

    // ���͂��ꂽ�l���㉺�̎�
    private bool InputValueUpDown(int value)
    {
        // �v���C���[�̈ړ��󋵂�ύX
        switch (value)
        {
            // ��
            case 1:
                moveState = PlayerMoveState.Up; // ��ɓ���
                CharactertRotateAnimation(new Vector3(-90f, 0f, 0f), rotateProductionTime);

                // ��ɏ�Q������������
                if(upHitDetection.GetMoveHitDetection()) return false;

                break; 

            // ��
            case -1:
                moveState = PlayerMoveState.Down; // ���ɓ���
                CharactertRotateAnimation(new Vector3(-90f, 180f, 0f), rotateProductionTime);

                // ���ɏ�Q������������
                if (downHitDetection.GetMoveHitDetection()) return false;
                break;

            default:
                Debug.LogError($"�z��O�̏㉺���͒l�G���[:" + value);
                return false;
        }

        return true;
    }

    // ���͂��ꂽ�l�����E�̎�
    private bool InputValueLeftRight(int value)
    {
        // �v���C���[�̈ړ��󋵂�ύX
        switch (value)
        {
            // �E
            case 1:
                moveState = PlayerMoveState.Right; // �E�ɓ���
                CharactertRotateAnimation(new Vector3(-90f, 90f, 0f), rotateProductionTime);

                // �E�ɏ�Q������������
                if (rightHitDetection.GetMoveHitDetection()) return false;
                break;

            // ��
            case -1:
                moveState = PlayerMoveState.Left; // ���ɓ���
                CharactertRotateAnimation(new Vector3(-90f, 270f, 0f), rotateProductionTime);

                // ���ɏ�Q������������
                if (leftHitDetection.GetMoveHitDetection()) return false;
                break;

            default:
                Debug.LogError($"�z��O�̍��E���͒l�G���[:" + value);
                return false;
        }

        return true;

    }

    /// <summary>
    /// �L�����N�^�[�̉�]�A�j���[�V����
    /// </summary>
    public void CharactertRotateAnimation(Vector3 rotate, float time)
    {
        // ��]
        transform.DORotate(
            rotate,                     // �I������Rotation
            time                        // ���o����
        )
        .SetEase(rotateEase)            // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .SetLink(gameObject);           // ����GameObject���폜���ꂽ�����Ŏ~�߂�悤�ɕR�t����
    }

    // �Q�[���I�u�W�F�N�g���m���ڐG�����^�C�~���O�Ŏ��s
    private void OnTriggerEnter(Collider other)
    {
        if (mainManager.GetGamePlayStatus() == MainManager.GamePlayStatus.Result) return;

        // �G�Ɠ���������
        if (other.gameObject.CompareTag("Enemy"))
        {
            this.moveTween.Kill();      // �ړ����~�߂�

            // ���W�Œ�
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, other.transform.position.z);

            // �Q�[���v���C�󋵂��u���ʁv�ɂ���
            mainManager.SetGamePlayStatus(MainManager.GamePlayStatus.Result);

            ViralEnemyManager enemy = other.GetComponent<ViralEnemyManager>();
            enemy.ExplosionEffectGeneration();  // �����G�t�F�N�g����
            mainManager.GameOverResultDisplay(enemy.GetEnemyExplanation());    // �Q�[���I�[�o�[���ʂ�\��
            Destroy(enemy.gameObject); // �폜

            // �����ł̎��SSE
            SESound.ExplosionDeathSE();

            // �L�����N�^�[���Ђ�����Ԃ�
            CharactertRotateAnimation(deathRotate, deathProductionTime);
        }

        // �S�[����
        else if (other.CompareTag("GoalLineArea"))
        {
            // �Q�[���v���C�󋵂��u���ʁv�ɂ���
            mainManager.SetGamePlayStatus(MainManager.GamePlayStatus.Result);
            mainManager.GameGoalResultDisplay();    // �S�[�����ʂ�\��
        }

        // �r�[���Ɠ���������
        else if (other.gameObject.CompareTag("Beam"))
        {
            this.moveTween.Kill();      // �ړ����~�߂�

            // ���W�Œ�
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, other.transform.position.z);

            // �Q�[���v���C�󋵂��u���ʁv�ɂ���
            mainManager.SetGamePlayStatus(MainManager.GamePlayStatus.Result);

            BeamManager beam = other.GetComponent<BeamManager>();
            mainManager.GameOverResultDisplay(beam.GetBeamExplanation());    // �Q�[���I�[�o�[���ʂ�\��

            //�r�[���ł̎��SSE
            SESound.BeamDeathSE();

            // �L�����N�^�[���Ђ�����Ԃ�
            CharactertRotateAnimation(deathRotate, deathProductionTime);
        }

        // �T�C�o�[�U���Ɠ���������
        else if (other.gameObject.CompareTag("CyberAttack"))
        {
            this.moveTween.Kill();      // �ړ����~�߂�

            // ���W�Œ�
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, other.transform.position.z);

            // �Q�[���v���C�󋵂��u���ʁv�ɂ���
            mainManager.SetGamePlayStatus(MainManager.GamePlayStatus.Result);

            CyberAttackManager cyberAttack = other.GetComponent<CyberAttackManager>();
            mainManager.GameOverResultDisplay(cyberAttack.GetBeamExplanation());    // �Q�[���I�[�o�[���ʂ�\��

            // �T�C�o�[�U���ł̎��SSE
            SESound.CyberAttackDeathSE();

            // �L�����N�^�[���Ђ�����Ԃ�
            CharactertRotateAnimation(deathRotate, deathProductionTime);
        }

    }

    /// <summary>
    /// �L�����N�^�[�̈ړ��󋵂�Ԃ�
    /// </summary>
    public bool PlayerMoveStopCheck()
    {
        if(moveState == PlayerMoveState.Stop) return true;

        return false;
    }

}
