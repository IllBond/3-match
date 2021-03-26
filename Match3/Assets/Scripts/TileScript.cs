using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public GameSetting Setting; // Обьект с настройками

    public SpriteRenderer spriteRenderer;
    public bool isSelected; // Выбран сейчсас тайл?
    public bool isSwap; // Свапаеться ли сейчас тайл?
    public bool isSoftSize; // плавное появление спрайта 

    public bool isEmpty // Пустой ли сейчас тайл?
    {
        get
        {
            return spriteRenderer.sprite == null ? true : false;
        }
    }

    private Vector3 newPos; // Позиция тайла которую нужно принять

    private Vector3 startSize; // Текущий размер тайла
    private Vector3 newSize; // Нормальный размер тайла

    void Start()
    {
        Setting = GameObject.FindGameObjectWithTag("Setting").GetComponent<GameSetting>();
        startSize = new Vector3(0.5f, 0.5f, 0.5f); 
        newSize = new Vector3(1f, 1f, 1f); 
    }


    void Update()
    {

        // При включенном isSwap происходит плавный свайп
        if (isSwap)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, 0.04f);

            // Если тайл принимает нужную позицию:
            if (transform.position == newPos)
            {
                isSwap = false; // isSwap выключается
                transform.position = newPos; 
                Setting.swaping.Remove(this); //Убираем из списка того что нужно в данный момент свайпается
                Setting.swaped.Add(this); // Добавляем в список свайпнутых 

                if (this.isEmpty) {
                    SetNewSprite(this); // Если после завершения свпйпа тайл пустой то добавим ему спрайт
                }
            }
        }

        // При включенном isSoftSize спрайт увеличивается от 0.5 к 1.0
        if (isSoftSize)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, newSize, 0.04f);

            if (transform.localScale == newSize)
            {
                isSoftSize = false; // Выключаем isSoftSize
            }
        }

    }

    public void SwapTwoTiles(Vector3 newCoor)
    {
        isSwap = true; // Включаем isSwap
        newPos = newCoor; // Новый кординат в который нужно переместиться
        Setting.swaping.Add(this); // Добавить в список того что свайпается
    }

    public void SoftSize()
    {
        isSoftSize = true; // Включаем isSoftSize
        transform.localScale = startSize; // Уменьшаем размер что бы он плавно увеличивался
    }


    public void SetNewSprite(TileScript tile)
    {
        List<Sprite> tileSprite = new List<Sprite>(); // Список того какие спрайты можно использовать 
        tileSprite.AddRange(Setting.tileSprite); // Сюда добавляем список из настроек

        // Setting.dirRay 4 направления вверх, вниз, влево, вправо
        for (int i = 0; i < Setting.dirRay.Length; i++)
        {
            // Из tile пускаем луч, в (вверх, вниз, влево или вправо)
            RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Setting.dirRay[i]);
            {
                // Если ничего не встретили то ничего не делаем
                // Если встречаем обьект убираем из списка возможных спрайтов красный синий или другие цвета
                if (hit.collider != null)
                {
                    tileSprite.Remove(hit.collider.gameObject.GetComponent<TileScript>().spriteRenderer.sprite);
                }
            }
        }

        // Вставляем спрайт пустому тайлу с учетом окружения
        tile.spriteRenderer.sprite = tileSprite[Random.Range(0, tileSprite.Count - 1)];
        tile.SoftSize(); // Включаем плавное увеличение от 0.5 дл 1.0
        Setting.empty.Remove(tile); // Удаляем из списка пустых тайлов
    }
}
