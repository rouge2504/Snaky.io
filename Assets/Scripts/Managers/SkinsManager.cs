using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinsManager : MonoBehaviour
{
    public static SkinsManager instance;
    public enum Type { NONE, ANIMALS, CHARACTERS, FACES, FLAGS, PRO, EGGS}
    public Type type;

    List<SkinMask> skinMasks;

    public GameObject maskObjectPrefab;
    public GameObject colorObjectPrefab;

    public Transform contentMaskObjects;
    public Transform contentColorObjects;

    public Button[] masksButtons;

    public Sprite selectedSprite;
    public Sprite deselectedSprite;

    public GameObject[] colorRender;

    public Image MaskSnake;

    public Button buyEggs;

    public Color[] snankeColors;

    private List<Color> colorOnSnake;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        skinMasks = CSVReader.Instance.Read();
        buyEggs.gameObject.SetActive(false);
        
        GetMaskByType((int)type);
        print(skinMasks[0].nameMask);
        GetColors();
        colorOnSnake = new List<Color>();
        for (int i = 0; i < colorRender.Length; i++)
        {
            colorRender[i].gameObject.GetComponent<Floater>().enabled = false;
        }
    }

    public void OpenWindow()
    {
        StartCoroutine(StartAnimation());
    }

    public void CloseWindow()
    {
        for (int i = 0; i < colorRender.Length; i++)
        {
            colorRender[i].gameObject.GetComponent<Floater>().enabled = false;
        }

        PlayerProgress.instance.SetColorOnMainUI();
        PlayerProgress.instance.SetMaskOnUI();
    }


    public void GetColors()
    {
        for (int i = 0; i < snankeColors.Length; i++)
        {
            ColorObject colorObject = Instantiate(colorObjectPrefab, contentColorObjects).GetComponent<ColorObject>();
            colorObject.color.color = snankeColors[i];
            colorObject.closeButton.gameObject.SetActive(false);
            colorObject.button.onClick.AddListener(() => SetColor(colorObject));
        }
    }

    public void RemoveColor(ColorObject colorObject)
    {
        colorOnSnake.Remove(colorObject.color.color);
        colorObject.closeButton.gameObject.SetActive(false);
        colorObject.closeButton.onClick.RemoveAllListeners();
        SetColorOnSnake(colorOnSnake.Count == 1 ? colorOnSnake[0] : colorObject.color.color);
        PlayerProgress.instance.colorOnSnake = colorOnSnake;
    }

    public void SetColor(ColorObject colorObject)
    {
        colorOnSnake.Add(colorObject.color.color);
        if (colorOnSnake.Count > 3)
        {
            colorOnSnake.RemoveAt(colorOnSnake.Count - 1);
            return;
        }
        colorObject.closeButton.gameObject.SetActive(true);

        colorObject.closeButton.onClick.AddListener(() => RemoveColor(colorObject));
        SetColorOnSnake(colorObject.color.color);
        PlayerProgress.instance.colorOnSnake = colorOnSnake;
    }

    private void SetColorOnSnake(Color color)
    {

        for (int i = 0; i < colorRender.Length; i++)
        {
            colorRender[i].GetComponent<Image>().color = color;

            if (colorOnSnake.Count == 2)
            {
                if (i % 2 != 0)
                {
                    colorRender[i].GetComponent<Image>().color = colorOnSnake[0];
                }
                else
                {
                    colorRender[i].GetComponent<Image>().color = colorOnSnake[1];
                }
            }
            else if (colorOnSnake.Count == 3)
            {
                int it = i + 1;



                if (it % 2 != 0) //Rojo
                {
                    colorRender[i].GetComponent<Image>().color = colorOnSnake[0];
                }
                else if (it % 2 == 0) // Amarillo
                {
                    colorRender[i].GetComponent<Image>().color = colorOnSnake[1];
                }

                if (it > 2)
                {
                    if (it % 2 != 0) //Amarillo
                    {
                        colorRender[i].GetComponent<Image>().color = colorOnSnake[1];
                    }
                    else if (it % 2 == 0) // Rojo
                    {
                        colorRender[i].GetComponent<Image>().color = colorOnSnake[0];
                    }
                }

                if (it % 3 == 0) //Verde
                {
                    colorRender[i].GetComponent<Image>().color = colorOnSnake[2];

                }
            }
        }
    }

    public void Check(Image type)
    {
        print(type);
    }

    public void GetMaskByType(int type)
    {
        for (int i = 0; i < masksButtons.Length; i++)
        {
            masksButtons[i].GetComponent<Image>().sprite = deselectedSprite;
        }

        masksButtons[type - 1].GetComponent<Image>().sprite = selectedSprite;

        foreach (Transform child in contentMaskObjects)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < skinMasks.Count; i++)
        {
            if (type == skinMasks[i].type)
            {
                SkinMask skinMask = skinMasks[i];
               /* string url = "Masks/" + skinMask.url + "/" + skinMask.nameMask;
                print(url);
                Sprite texture = Resources.Load<Sprite>(url);*/

                MaskObject maskObject = Instantiate(maskObjectPrefab, contentMaskObjects).GetComponent<MaskObject>();
                maskObject.maskImage.sprite = skinMask.maskSprite;
                maskObject.buttonImage.onClick.AddListener(() => SetMask(skinMask));

            }
        }
    }

    public void SetMask(SkinMask skinMask)
    {
        MaskSnake.sprite = skinMask.maskSprite;
        PlayerProgress.instance.skinMask = skinMask;
        buyEggs.gameObject.SetActive(skinMask.eggValue != 0 ? true : false);
        buyEggs.GetComponentInChildren<Text>().text = skinMask.eggValue.ToString();
    }

    IEnumerator StartAnimation()
    {
        for (int i = 0; i < colorRender.Length; i++)
        {
            colorRender[i].gameObject.GetComponent<Floater>().enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

    }


    public Sprite GetRandomMask()
    {
        int rnd = Random.Range(-1, skinMasks.Count);

        return (-1 == rnd) ? null : skinMasks[rnd].maskSprite;
    }
}
