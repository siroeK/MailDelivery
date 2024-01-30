using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

/// <summary>
/// �X�e�[�W�����Ǘ��̃v���W�F�N�g
/// </summary>
public class StageCreationManager : MonoBehaviour
{
    [SerializeField, Header("�v���C���[�̃I�u�W�F�N�g�Q��")]
    private Transform player;

    [SerializeField, Header("���̊Ǘ��I�u�W�F�N�g")]
    private Transform roads = null;

    [SerializeField, Header("�����̈�̓�(�G���A)")]
    private GameObject[] invalidArea = new GameObject[4];

    [SerializeField, Header("���ʂ̓�")]
    private GameObject[] normalRoad = new GameObject[31];

    [SerializeField, Header("�G�̓�")]
    private GameObject[] enemyRoad = new GameObject[12];

    [SerializeField, Header("�r�[���̓�")]
    private GameObject[] beamRoad = new GameObject[4]; 

    [SerializeField, Header("�T�C�o�[�U���̓�")]
    private GameObject cyberAttackRoad = null;

    [Header("�����E�폜����֌W�̒l")]
    [SerializeField, Tooltip("���߂ɐ������铹�̒l")]
    private int firstCreationRoadVal = 30;

    [SerializeField, Tooltip("���ʂ̓��敪�̍ő吶���l")]
    private int maxNormalClassification = 2;

    [SerializeField, Tooltip("�W����̓��敪�̍ő吶���l")]
    private int maxHinderClassification = 5;

    [SerializeField, Tooltip("�����̈�̓�(�G���A)�������W")]
    private Vector3 invalidAreaPos = new Vector3(0f, 0f, -3f);

    private int invalidAreaCreation = 0;        // �������閳���̈�̓�(�G���A)

    private int normalRoadCreation = 0;         // �������镁�ʂ̓�

    private int firstNormalRoadCreation = 0;    // ���߂̐������镁�ʂ̓�

    private int enemyRoadCreation = 0;          // ��������G�̓�

    private int beamRoadCreation = 0;           // ��������r�[���̓�

    private int roadCreationVal = 2;            // ���̐����̒l

    private int hinderRoadTypesNum = 3;         // �W���铹�̎�ނ̐�

    private bool normalRoadCreationFlg = false; // �ʏ�̓����쐬���邩 

    [SerializeField, Tooltip("�����E�폜���鏊�̒l")]
    private int creatAndDeletPointVal = 15;

    [SerializeField, Tooltip("�����E�폜���鏊�̑����l")]
    private int creatAndDeletIncrementalVal = 15;

    /// <summary>
    /// �W���铹�̎��(���𑝂₵�Ċm������)
    /// </summary>
    public  enum HinderRoadTypes
    {
        enemy_1 = 0,        // �G
        enemy_2 = 1,        // �G
        beam = 2,           // �r�[��
        cyberAttack = 3,    // �T�C�o�[�U��
    }

    void Awake()
    {
        // �W���铹�̎�ނ̐�������
        hinderRoadTypesNum = System.Enum.GetNames(typeof(HinderRoadTypes)).Length;

        // ���߂̃X�e�[�W�𐶐�
        StartStageCreation();
    }

    // Update is called once per frame
    void Update()
    {
        // ���̐����E�폜����
        if (creatAndDeletPointVal == player.localPosition.z)
        {
            // ���̐���
            StartRoadCreation(creatAndDeletPointVal + firstCreationRoadVal);    // �����E�폜���鏊�̒l + ���߂ɐ������铹�̒l

            // ���̍폜
            // �q�I�u�W�F�N�g���擾����
            var roadChildren = GetChildren(roads);
            // �擾�����q�I�u�W�F�N�g�������O�o��
            for (var i = 0; i < roadChildren.Length; i++)
            {
                // �l���ׂď�����������폜����
                if (roadChildren[i].position.z < (creatAndDeletPointVal - creatAndDeletIncrementalVal))
                {
                    Destroy(roadChildren[i].gameObject); // ���̃I�u�W�F�N�g���폜
                }
            }

            // �����E�폜���鏊�̒l�𑝂₷
            creatAndDeletPointVal += creatAndDeletIncrementalVal;
        }
    }

    /// <summary>
    /// ���߂̃X�e�[�W�𐶐�
    /// </summary>
    private void StartStageCreation()
    {
        FirstInvalidAreaCreation();
        FirstNormalCreation();
        StartRoadCreation(firstCreationRoadVal);
    }


    /// <summary>
    /// ���߂̒ʏ�̓��̐���
    /// </summary>
    private void FirstNormalCreation()
    {
        for (int i = 1; i >= -2; i--)
        {
            firstNormalRoadCreation = Random.Range(0, normalRoad.Length);
            Instantiate(normalRoad[firstNormalRoadCreation], new Vector3(0f, 0.5f, i), Quaternion.identity, roads);
        }
    }

    /// <summary>
    /// ���߂̖����̈�̐���
    /// </summary>
    private void FirstInvalidAreaCreation()
    {
        invalidAreaCreation = Random.Range(0, invalidArea.Length);  
        Instantiate(invalidArea[invalidAreaCreation], new Vector3(0f, 0.5f, -3f), Quaternion.identity, roads);
    }

    /// <summary>
    ///�@���������n�߂�
    /// </summary>
    private void StartRoadCreation(int creationVal)
    {
        // ������
        while (roadCreationVal < creationVal)
        {
            // ������
            RoadCreation();
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    private void RoadCreation()
    {
        // �ʏ�̓��𐶐�����
        if (normalRoadCreationFlg)
        {
            // ���̐��������
            int creationVal = Random.Range(1, maxNormalClassification + 1); // 1 �` maxNormalClassification

            for (int i = 0; i < creationVal; i++)
            {
                NormalRoadCreation(roadCreationVal);
                roadCreationVal++;  // ���̐����̒l�𑝂₷
            }
        }
        // �W���铹�𐶐����� 
        else
        {
            HinderRoadCreation();
        }

        // �ʏ�̓����쐬���邩
        normalRoadCreationFlg = !normalRoadCreationFlg;
    }

    /// <summary>
    /// �W���铹�𐶐�����
    /// </summary>
    private void HinderRoadCreation()
    {
        // ���̐��������
        int creationVal = Random.Range(1, maxHinderClassification + 1); // 1 �` maxHinderClassification

        for (int i = 0; i < creationVal; i++)
        {
            // �W���铹�̐���
            int roadType = Random.Range(0, hinderRoadTypesNum);

            // �W���铹�̎��
            switch (roadType)
            {
                // �G
                case (int)HinderRoadTypes.enemy_1:
                    EnemyRoadCreation(roadCreationVal);
                    break;

                // �G
                case (int)HinderRoadTypes.enemy_2:
                    EnemyRoadCreation(roadCreationVal);
                    break;

                // �r�[��
                case (int)HinderRoadTypes.beam:
                    BeamRoadCreation(roadCreationVal);
                    break;

                // �T�C�o�[�U��
                case (int)HinderRoadTypes.cyberAttack:
                    CyberAttackRoadCreation(roadCreationVal);
                    break;

                default:
                    Debug.LogError("�W���铹�̐����̒l���Ԉ���Ă��܂��B" + roadType);
                    break;
            }

            roadCreationVal++;  // ���̐����̒l�𑝂₷
        }
    }

    /// <summary>
    /// �ʏ�̓�����
    /// </summary>
    private void NormalRoadCreation(int posZ)
    {
        normalRoadCreation = Random.Range(0, normalRoad.Length);
        Instantiate(normalRoad[normalRoadCreation], new Vector3(0f, 0.5f, posZ), Quaternion.identity, roads);
    }

    /// <summary>
    /// �G�̓�����
    /// </summary>
    private void EnemyRoadCreation(int posZ)
    {
        enemyRoadCreation = Random.Range(0, enemyRoad.Length);
        Instantiate(enemyRoad[enemyRoadCreation], new Vector3(0f, 0.5f, posZ), Quaternion.identity, roads);
    }

    /// <summary>
    /// �r�[���̓�����
    /// </summary>
    private void BeamRoadCreation(int posZ)
    {
        beamRoadCreation = Random.Range(0, beamRoad.Length);
        Instantiate(beamRoad[beamRoadCreation], new Vector3(0f, 0.5f, posZ), Quaternion.identity, roads);
    }

    /// <summary>
    /// �T�C�o�[�U���̓�����
    /// </summary>
    private void CyberAttackRoadCreation(int posZ)
    {
        Instantiate(cyberAttackRoad, new Vector3(0f, 0.5f, posZ), Quaternion.identity, roads);
    }

    // parent�����̎q�I�u�W�F�N�g��for���[�v�Ŏ擾����
    private static Transform[] GetChildren(Transform parent)
    {
        // �q�I�u�W�F�N�g���i�[����z��쐬
        var children = new Transform[parent.childCount];

        // 0�`��-1�܂ł̎q�����Ԃɔz��Ɋi�[
        for (var i = 0; i < children.Length; ++i)
        {
            children[i] = parent.GetChild(i);
        }

        // �q�I�u�W�F�N�g���i�[���ꂽ�z��
        return children;
    }

}
