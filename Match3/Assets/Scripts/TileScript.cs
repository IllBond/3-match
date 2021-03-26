using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{

    public GameObject S_Setting;
    public GameSetting Setting;

    public SpriteRenderer spriteRenderer;
    public bool isSelected; // Выбран сейчсас тайл?
    public bool isSwap; // Свапаеться сейчас тайл?
    public bool isSoftSize; // Появление 

    public bool isEmpty // Пустой ли сейчас тайл?

    {
        get
        {
            return spriteRenderer.sprite == null ? true : false;
        }
    }


    // Переменные с позициями для свайпа 2х тайлов
    private Vector3 newPos;
    private Vector3 oldPos;

    private TileScript newTile;
    private TileScript oldTile;

    private Vector3 startSize;
    private Vector3 newSize;

    void Start()
    {
        S_Setting = GameObject.FindGameObjectWithTag("Setting");
        Setting = S_Setting.GetComponent<GameSetting>();

        newPos = transform.position;
        oldPos = transform.position;
        
        startSize = transform.localScale;
        newSize = transform.localScale;
    }



    void Update()
    {

        if (isSwap)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, 0.1f);

            if (transform.position == newPos)
            {
                isSwap = false;
                transform.position = newPos;
                Setting.swaping.Remove(this);
                Setting.swaped.Add(this);

                if (this.isEmpty) {
                    SetNewSprite(this);
                    
                }
            }
        }
        

        if (isSoftSize)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, newSize, 0.1f);

            if (transform.localScale == newSize)
            {
                isSoftSize = false;
            }
        }

    }

    public void SwapTwoTiles(Vector3 newCoor)
    {
        isSwap = true;
        oldTile = this;
        oldPos = transform.position;
        newPos = newCoor;

        Setting.swaping.Add(this);
    }

    public void SoftSize()
    {
        isSoftSize = true;
        startSize = new Vector3(0.5f, 0.5f, 0.5f);
        newSize = new Vector3(1f, 1f, 1f);
        transform.localScale = startSize;
    }


    public void SetNewSprite(TileScript tile)
    {
        if (tile.spriteRenderer.sprite == null)
        {
            List<Sprite> tileSprite = new List<Sprite>();
            tileSprite.AddRange(Setting.tileSprite); // Список тайлов

            for (int i = 0; i < Setting.dirRay.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Setting.dirRay[i]);
                {
                    if (hit.collider != null)
                    {
                        tileSprite.Remove(hit.collider.gameObject.GetComponent<TileScript>().spriteRenderer.sprite);
                    }
                }
            }

            tile.spriteRenderer.sprite = tileSprite[Random.Range(0, tileSprite.Count - 1)];
            tile.SoftSize();
            //Debug.Log(Setting.empty.Count);
            Setting.empty.Remove(tile);
            
        }


    }
}
