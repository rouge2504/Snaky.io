using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinsManager : MonoBehaviour
{
    public static SkinsManager instance;


    public enum Type { NONE, ANIMALS, CHARACTERS, FACES, FLAGS, PRO, EGGS}
    public Type type;

    public Sprite defaultMask;
    public Material defaultMaterial;
    public Material defaultSprintMaterial;
    public Material defaultMaskMaterial;
    public Color defaultColor;

    List<SkinMask> skinMasks;

    public GameObject maskObjectPrefab;
    public GameObject colorObjectPrefab;

    public Transform contentMaskObjects;
    public Transform contentColorObjects;

    public Button[] masksButtons;

    public Sprite selectedSprite;
    public Sprite deselectedSprite;

    public Button buyTransparency;
    public Slider sliderTransparency;

    public float Transparency
    {
        set
        {
            float trasparency = value;
            if (trasparency < 0.2f)
            {
                trasparency = 0.2f;
            }
            GamePrefs.PLAYER_TRASNPARENCY = trasparency;

            for (int i = 0; i < colorRender.Length; i++)
            {
                Color color = colorRender[i].GetComponent<Image>().color;
                colorRender[i].GetComponent<Image>().color = new Color(color.r, color.g, color.b, trasparency);
            }
        }
    }

    public GameObject[] colorRender;

    public Image MaskSnake;

    public Button buyEggs;

    public Color[] snankeColors;

    private List<Color> colorOnSnake;

    public List<Material> materialColors;
    public List<Material> materialSprintColors;

    public List<Material> teamColors;
    private List<Material> materialOnSnake;


    public Color GetDefaultColor ()
    {
        return new Color (0,1,0,1);
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        skinMasks = CSVReader.Instance.Read();
        buyEggs.gameObject.SetActive(false);
        GetMaskByType((int)type);
        print(skinMasks[0].nameMask);
        colorOnSnake = new List<Color>();
        for (int i = 0; i < colorRender.Length; i++)
        {
            colorRender[i].gameObject.GetComponent<Floater>().enabled = false;
        }

        bool transparency = GamePrefs.GetBool(GameUtils.TRANSPARENCY, 0);

        UpdateTransparencyState(transparency);


        Init();
        GetColors();
    }

    public void UpdateTransparencyState(bool active)
    {
        buyTransparency.gameObject.SetActive(!active);
        sliderTransparency.value = GamePrefs.PLAYER_TRASNPARENCY;
        sliderTransparency.interactable = active;
    }

    public void Init()
    {
        PlayerProgress.instance.colorOnSnake = StorageManager.Singleton.LoadColors();
        if (PlayerProgress.instance.colorOnSnake.Count > 0)
        {
            colorOnSnake = PlayerProgress.instance.colorOnSnake;
            SetColorOnSnake();
            //PlayerProgress.instance.materialSprintOnSnake = materialSprintColors;

            for (int i = 0; i < PlayerProgress.instance.colorOnSnake.Count; i++)
            {
                /*Material material = materialColors[SetColorOnSnake(i, PlayerProgress.instance.colorOnSnake.Count)];
                Material materialSprint = materialSprintColors[SetColorOnSnake(i, PlayerProgress.instance.colorOnSnake.Count)];
                material.color = PlayerProgress.instance.colorOnSnake[i];
                materialSprint.color = PlayerProgress.instance.colorOnSnake[i];*/
                PlayerProgress.instance.materialOnSnake.Add(materialColors[GetMaterialForColor(PlayerProgress.instance.colorOnSnake[i])]);
                PlayerProgress.instance.materialSprintOnSnake.Add(materialSprintColors[GetMaterialForColor(PlayerProgress.instance.colorOnSnake[i])]);
            }
        }
        else
        {
            colorOnSnake = PlayerProgress.instance.colorOnSnake;
            SetColorOnSnake();
            //PlayerProgress.instance.materialSprintOnSnake = materialSprintColors;

            for (int i = 0; i < PlayerProgress.instance.colorOnSnake.Count; i++)
            {
                /*Material material = materialColors[SetColorOnSnake(i, PlayerProgress.instance.colorOnSnake.Count)];
                Material materialSprint = materialSprintColors[SetColorOnSnake(i, PlayerProgress.instance.colorOnSnake.Count)];
                material.color = PlayerProgress.instance.colorOnSnake[i];
                materialSprint.color = PlayerProgress.instance.colorOnSnake[i];*/
                /*Material material = new Material(defaultMaterial);
                Material materialSprint = new Material(defaultSprintMaterial);*/
                Material sprint = new Material(materialSprintColors[GetMaterialForColor(Color.green)]);
                PlayerProgress.instance.materialOnSnake.Add(materialColors[GetMaterialForColor(Color.green)]);
                PlayerProgress.instance.materialSprintOnSnake.Add(sprint);
            }
        }
    }

    public int GetMaterialForColor(Color color)
    {
        for(int i = 0; i < materialColors.Count; i++)
        {
            if (color.r == materialColors[i].color.r && color.g == materialColors[i].color.g && color.b == materialColors[i].color.b)
            {
                return i;
            }
        }

        return 0;
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

            foreach (Color color in PlayerProgress.instance.colorOnSnake)
            {
                if (color == snankeColors[i])
                {
                    SetColor(colorObject, materialColor, materialSprintColor, true);
                    
                }
            }
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

    public void SetColor(ColorObject colorObject, Material material, Material materialSprint, bool choosed = false)
    {
        if (AchievementManager.instance != null)
        {
            AchievementManager.instance.EditSnake();
        }
        if (!choosed)
        {
            colorOnSnake.Add(colorObject.color.color);
        }
        if (colorOnSnake.Count > 3)
        {
            colorOnSnake.RemoveAt(colorOnSnake.Count - 1);
            return;
        }
        colorObject.closeButton.gameObject.SetActive(true);

        colorObject.closeButton.onClick.AddListener(() => RemoveColor(colorObject, material, materialSprint));
        if (!choosed)
        {
            SetColorOnSnake(colorObject.color.color);
            PlayerProgress.instance.colorOnSnake = colorOnSnake;
            StorageManager.Singleton.SaveColors(colorOnSnake);
            Material sprint = new Material(materialSprint);
            PlayerProgress.instance.materialOnSnake.Add(material);
            PlayerProgress.instance.materialSprintOnSnake.Add(sprint);

        }
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
                MaskObject maskObject = Instantiate(maskObjectPrefab, contentMaskObjects).GetComponent<MaskObject>();
                maskObject.maskName = skinMask.nameMask;
                maskObject.maskImage.sprite = skinMask.maskSprite;
                maskObject.SetImageOnLock();
                maskObject.unlocked = SetUnlock(skinMask);
                maskObject.lockImage.SetActive(!SetUnlock(skinMask));
                maskObject.buttonImage.onClick.AddListener(() => SetMask(maskObject, skinMask));

            }
        }
    }

    public bool SetUnlock(SkinMask skinMask)
    {
        return (Type)skinMask.type != Type.PRO ? true : GamePrefs.GetBool(skinMask.id, 0);
    }

    public void SetMask(MaskObject maskObject, SkinMask skinMask)
    {
        buyEggs.onClick.RemoveAllListeners();
        if (!maskObject.unlocked)
        {
            DialogueManager.instance.PopUp("Unlock this Mask!\n" + skinMask.description);
            return;
        }
        buyEggs.gameObject.SetActive(skinMask.eggValue != 0 ? true : false);
        if (skinMask.eggValue != 0)
        {
            buyEggs.onClick.AddListener(() => SelectBuyEgg(skinMask));
            buyEggs.GetComponentInChildren<Text>().text = skinMask.eggValue.ToString();
            return;
        }
        MaskSnake.sprite = skinMask.maskSprite;
        PlayerProgress.instance.skinMask = skinMask;
        GamePrefs.MASK = skinMask.nameMask;

    }

    public void SelectBuyEgg(SkinMask skinMask)
    {
        if (StoreManager.instance.DecreaseEggs(skinMask))
        {
            MaskSnake.sprite = skinMask.maskSprite;
            PlayerProgress.instance.skinMask = skinMask;
            GamePrefs.MASK = skinMask.nameMask;
        }
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
