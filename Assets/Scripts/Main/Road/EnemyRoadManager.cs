using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�l�~�[���o�Ă��铹�̊Ǘ��v���W�F�N�g
/// </summary>
public class EnemyRoadManager : MonoBehaviour
{
    [SerializeField, Header("��������G�l�~�[�I�u�W�F�N�g")]
    private GameObject enemyObject;

    private Transform enemyListObj; // �G�l�~�[�̃��X�g�I�u�W�F�N�g

    [Header("���E�̖ڕW�̃u���b�N���W")]
    [SerializeField, Tooltip("��ԉE�̃u���b�N���W")]
    private Transform rightMostBlockPos;

    [SerializeField, Tooltip("��ԍ��̃u���b�N���W")]
    private Transform leftMostBlockPos;

    [Header("�G�̐������")]
    [SerializeField, Tooltip("�G�̃X�^�[�g���鑤�ʕ���")]
    private StartGenerationSide startSide = StartGenerationSide.Right;

    [SerializeField, Tooltip("�G�̍ŏ��������Ԃ̊Ԋu")]
    private float minGenerationTime = 1.0f;

    [SerializeField, Tooltip("�G�̍ő吶�����Ԃ̊Ԋu")]
    private float maxGenerationTime = 3.0f;

    private float generationTime = 0.0f;    // ��������

    private Vector3 rightQuaternion = new Vector3(0f, -90f, 0f);

    private Vector3 leftQuaternion = new Vector3(0f, 90f, 0f);

    /// <summary>
    /// �������X�^�[�g���鑤��
    /// </summary>
    private enum StartGenerationSide
    { 
        None,
        Right,
        Left
    }

    private void Awake()
    {
        // �������Ԃ�����
        generationTime = 0f;

        // �Q�[���I�u�W�F�N�g�Q��
        enemyListObj = GameObject.FindWithTag("EnemyList").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(generationTime < 0.0f)
        {
            // �������Ԃ�����
            generationTime = Random.Range(minGenerationTime, maxGenerationTime + 1.0f);

            // �G�̐���
            if (startSide == StartGenerationSide.Right)
            {
                GameObject gameObject =  Instantiate(enemyObject, new Vector3(rightMostBlockPos.position.x, 1f, rightMostBlockPos.position.z), Quaternion.Euler(rightQuaternion), enemyListObj);
                ViralEnemyManager enemy = gameObject.GetComponent<ViralEnemyManager>();
                enemy.HorizontalAxisMove(leftMostBlockPos.position.x - 1.0f);
            }
            else if(startSide == StartGenerationSide.Left)
            {
                GameObject gameObject = Instantiate(enemyObject, new Vector3(leftMostBlockPos.position.x, 1f, leftMostBlockPos.position.z), Quaternion.Euler(leftQuaternion), enemyListObj);
                ViralEnemyManager enemy = gameObject.GetComponent<ViralEnemyManager>();
                enemy.HorizontalAxisMove(rightMostBlockPos.position.x + 1.0f);
            }
        }
        else
        {
            generationTime -= Time.deltaTime;   
        }

    }
}
