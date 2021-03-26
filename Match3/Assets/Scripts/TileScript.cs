using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public GameSetting Setting; // ������ � �����������

    public SpriteRenderer spriteRenderer;
    public bool isSelected; // ������ ������� ����?
    public bool isSwap; // ���������� �� ������ ����?
    public bool isSoftSize; // ������� ��������� ������� 

    public bool isEmpty // ������ �� ������ ����?
    {
        get
        {
            return spriteRenderer.sprite == null ? true : false;
        }
    }

    private Vector3 newPos; // ������� ����� ������� ����� �������

    private Vector3 startSize; // ������� ������ �����
    private Vector3 newSize; // ���������� ������ �����

    void Start()
    {
        Setting = GameObject.FindGameObjectWithTag("Setting").GetComponent<GameSetting>();
        startSize = new Vector3(0.5f, 0.5f, 0.5f); 
        newSize = new Vector3(1f, 1f, 1f); 
    }


    void Update()
    {

        // ��� ���������� isSwap ���������� ������� �����
        if (isSwap)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, 0.04f);

            // ���� ���� ��������� ������ �������:
            if (transform.position == newPos)
            {
                isSwap = false; // isSwap �����������
                transform.position = newPos; 
                Setting.swaping.Remove(this); //������� �� ������ ���� ��� ����� � ������ ������ ����������
                Setting.swaped.Add(this); // ��������� � ������ ���������� 

                if (this.isEmpty) {
                    SetNewSprite(this); // ���� ����� ���������� ������ ���� ������ �� ������� ��� ������
                }
            }
        }

        // ��� ���������� isSoftSize ������ ������������� �� 0.5 � 1.0
        if (isSoftSize)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, newSize, 0.04f);

            if (transform.localScale == newSize)
            {
                isSoftSize = false; // ��������� isSoftSize
            }
        }

    }

    public void SwapTwoTiles(Vector3 newCoor)
    {
        isSwap = true; // �������� isSwap
        newPos = newCoor; // ����� �������� � ������� ����� �������������
        Setting.swaping.Add(this); // �������� � ������ ���� ��� ����������
    }

    public void SoftSize()
    {
        isSoftSize = true; // �������� isSoftSize
        transform.localScale = startSize; // ��������� ������ ��� �� �� ������ ������������
    }


    public void SetNewSprite(TileScript tile)
    {
        List<Sprite> tileSprite = new List<Sprite>(); // ������ ���� ����� ������� ����� ������������ 
        tileSprite.AddRange(Setting.tileSprite); // ���� ��������� ������ �� ��������

        // Setting.dirRay 4 ����������� �����, ����, �����, ������
        for (int i = 0; i < Setting.dirRay.Length; i++)
        {
            // �� tile ������� ���, � (�����, ����, ����� ��� ������)
            RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Setting.dirRay[i]);
            {
                // ���� ������ �� ��������� �� ������ �� ������
                // ���� ��������� ������ ������� �� ������ ��������� �������� ������� ����� ��� ������ �����
                if (hit.collider != null)
                {
                    tileSprite.Remove(hit.collider.gameObject.GetComponent<TileScript>().spriteRenderer.sprite);
                }
            }
        }

        // ��������� ������ ������� ����� � ������ ���������
        tile.spriteRenderer.sprite = tileSprite[Random.Range(0, tileSprite.Count - 1)];
        tile.SoftSize(); // �������� ������� ���������� �� 0.5 �� 1.0
        Setting.empty.Remove(tile); // ������� �� ������ ������ ������
    }
}
