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

    public List<Material> materialColors;
    public List<Material> materialSprintColors;

    public List<Material> teamColors;
    private List<Material> materialOnSnake;

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

        if (PlayerProgress.instance.colorOnSnake.Count > 0)
        {
            colorOnSnake = PlayerProgress.instance.colorOnSnake;
            SetColorOnSnake();
            PlayerProgress.instance.materialSprintOnSnake = materialSprintColors;

            for (int i = 0; i < PlayerProgress.instance.colorOnSnake.Count; i++)
            {
                Material material = materialColors[0];
                Material materialSprint = materialSprintColors[0];
                material.color = PlayerProgress.instance.colorOnSnake[i];
                materialSprint.color = PlayerProgress.instance.colorOnSnake[i];
                PlayerProgress.instance.materialOnSnake.Add(material);
                PlayerProgress.instance.materialSprintOnSnake.Add(materialSprint);
            }
            /*for (int i = 0; i < PlayerProgress.instance.colorOnSnake.Count; i++)
            {
                materialSprintColors[i]
            }*/
        }
    }

    public void OpenWindow()
    {
        StartCoroutine(StartAnimation());
        if (PlayerProgress.instance.skinMask != null)
        {
            MaskSnake.sprite = PlayerProgress.instance.skinMask.maskSprite;
        }


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
            Material materialColor = materialColors[i];
            Material materialSprintColor = materialSprintColors[i];
            colorObject.button.onClick.AddListener(() => SetColor(colorObject, materialColor, materialSprintColor));


            //materialColors.Add(snankeMaterial[i]);

        }
    }

    public void RemoveColor(ColorObject colorObject, Material material, Material materialSprint)
    {
        colorOnSnake.Remove(colorObject.color.color);
        colorObject.closeButton.gameObject.SetActive(false);
        colorObject.closeButton.onClick.RemoveAllListeners();
        SetColorOnSnake(colorOnSnake.Count == 1 ? colorOnSnake[0] : colorObject.color.color);
        PlayerProgress.instance.colorOnSnake = colorOnSnake;
        StorageManager.Singleton.SaveColors(colorOnSnake);
        PlayerProgress.instance.materialOnSnake.Remove(material);
        PlayerProgress.instance.materialOnSnake.Remove(materialSprint);
    }

    public void SetColor(ColorObject colorObject, Material material, Material materialSprint)
    {
        colorOnSnake.Add(colorObject.color.color);
        if (colorOnSnake.Count > 3)
        {
            colorOnSnake.RemoveAt(colorOnSnake.Count - 1);
            return;
        }
        colorObject.closeButton.gameObject.SetActive(true);

        colorObject.closeButton.onClick.AddListener(() => RemoveColor(colorObject, material, materialSprint));
        SetColorOnSnake(colorObject.color.color);
        PlayerProgress.instance.colorOnSnake = colorOnSnake;
        StorageManager.Singleton.SaveColors(colorOnSnake);
        PlayerProgress.instance.materialOnSnake.Add(material);
        PlayerProgress.instance.materialSprintOnSnake.Add(materialSprint);
    }
    private void SetColorOnSnake()
    {

        for (int i = 0; i < colorRender.Length; i++)
        {
            if (colorOnSnake.Count == 1)
            {
                colorRender[i].GetComponent<Image>().color = colorOnSnake[0];

            }
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

    public Color SetColorOnSnake(int i, List<Color> colorOnSnake)
    {
        Color tempColor = Color.white;
        if (colorOnSnake.Count == 2)
        {
            if (i % 2 != 0)
            {
                tempColor = colorOnSnake[0];
            }
            else
            {
                tempColor = colorOnSnake[1];
            }
        }
        else if (colorOnSnake.Count == 3)
        {
            int it = i + 1;



            if (it % 2 != 0) //Rojo
            {
                tempColor = colorOnSnake[0];
            }
            else if (it % 2 == 0) // Amarillo
            {
                tempColor = colorOnSnake[1];
            }

            if (it > 2)
            {
                if (it % 2 != 0) //Amarillo
                {
                    tempColor = colorOnSnake[1];
                }
                else if (it % 2 == 0) // Rojo
                {
                    tempColor = colorOnSnake[0];
                }
            }

            if (it % 3 == 0) //Verde
            {
                tempColor = colorOnSnake[2];

            }
        }

        return tempColor;
    }

    public int SetColorOnSnake(int i, int Count)
    {
        int tempColor = 0;
        if (Count == 2)
        {
            if (i % 2 != 0)
            {
                tempColor = 0;
            }
            else
            {
                tempColor = 1;
            }
        }
        else if (Count == 3)
        {
            int it = i + 1;

            int flip = it / 3;

            bool isFlip = (flip % 2 != 0) ? false : true;

            if (!isFlip) {
                if (it % 2 != 0) //Rojo
                {
                    tempColor = 0;
                }
                else if (it % 2 == 0) // Amarillo
                {
                    tempColor = 1;
                }

            } else
            {
                if (it % 2 != 0) //Amarillo
                {
                    tempColor = 1;
                }
                else if (it % 2 == 0) // Rojo
                {
                    tempColor = 0;
                }
            }

            if (it % 3 == 0) //Verde
            {
                tempColor = 2;

            }
        }

        return tempColor;
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

    public Material[] GetMaterial()
    {
        Material[] materials = new Material[PlayerProgress.instance.colorOnSnake.Count];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = new Material(SnakeSpawner.Instance.sprintMat);
            materials[i].color = PlayerProgress.instance.colorOnSnake[i];
        }
        return materials;
    }
    public Sprite GetRandomMask()
    {
        int rnd = Random.Range(-1, skinMasks.Count);

        return (-1 == rnd) ? null : skinMasks[rnd].maskSprite;
    }
}
