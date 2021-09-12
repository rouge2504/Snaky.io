using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress instance;
    public SkinMask skinMask;
    public List<Color> colorOnSnake;
    public List<Material> materialOnSnake;
    public List<Material> materialSprintOnSnake;

    public GameObject[] colorRenderSnakeMainUI;
    public Image maskMainUI;

    private void Awake()
    {
        instance = this;
        CSVReader.Instance.Read();
        skinMask = CSVReader.Instance.Find(GamePrefs.MASK);
        if (skinMask != null)
        {
            SetMaskOnUI();
        }


    }

    private void Start()
    {

        colorOnSnake = StorageManager.Singleton.LoadColors();

        if (colorOnSnake.Count > 0)
        {
           // SkinsManager.instance.Init();
            SetColorOnMainUI();
        }
        else
        {

            colorOnSnake.Clear();
            colorOnSnake.Add(SkinsManager.instance.GetDefaultColor());
        }
        if (skinMask == null)
        {
            skinMask = new SkinMask();
            skinMask.maskSprite = SkinsManager.instance.defaultMask;
            skinMask.maskMaterial = SkinsManager.instance.defaultMaskMaterial;
            //SkinsManager.instance.Init();
        }


    }

    public void SetMaskOnUI()
    {
        maskMainUI.sprite = skinMask.maskSprite;
        GamePrefs.MASK= skinMask.nameMask;
    }

    public void SetColorOnMainUI()
    {
        for (int i = 0; i < colorRenderSnakeMainUI.Length; i++)
        {
            if (colorOnSnake.Count == 1)
            {
                colorRenderSnakeMainUI[i].GetComponent<Image>().color = colorOnSnake[0];
            }

            if (colorOnSnake.Count == 2)
            {
                if (i % 2 != 0)
                {
                    colorRenderSnakeMainUI[i].GetComponent<Image>().color = colorOnSnake[0];
                }
                else
                {
                    colorRenderSnakeMainUI[i].GetComponent<Image>().color = colorOnSnake[1];
                }
            }
            else if (colorOnSnake.Count == 3)
            {
                int it = i + 1;



                if (it % 2 != 0) //Rojo
                {
                    colorRenderSnakeMainUI[i].GetComponent<Image>().color = colorOnSnake[0];
                }
                else if (it % 2 == 0) // Amarillo
                {
                    colorRenderSnakeMainUI[i].GetComponent<Image>().color = colorOnSnake[1];
                }

                if (it > 2)
                {
                    if (it % 2 != 0) //Amarillo
                    {
                        colorRenderSnakeMainUI[i].GetComponent<Image>().color = colorOnSnake[1];
                    }
                    else if (it % 2 == 0) // Rojo
                    {
                        colorRenderSnakeMainUI[i].GetComponent<Image>().color = colorOnSnake[0];
                    }
                }

                if (it % 3 == 0) //Verde
                {
                    colorRenderSnakeMainUI[i].GetComponent<Image>().color = colorOnSnake[2];

                }
            }
        }
    }
}
