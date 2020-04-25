using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCreator_script : MonoBehaviour
{
    [System.Serializable]
    public class CharacterCreationSpritesClass
    {
        public string PartName;
        public Sprite PartSprite;        
    }

    public List<CharacterCreationSpritesClass> BackgroundGraphics;
    private int _backgroundIndex;
    private int _backgroundColorIndex;
    public List<CharacterCreationSpritesClass> BodyGraphics;
    private int _bodyIndex;
    private int _bodyColorIndex;
    public List<CharacterCreationSpritesClass> HeadGraphics;
    private int _headIndex;
    private int _headColorIndex;
    public List<CharacterCreationSpritesClass> EyesGraphics;
    private int _eyesIndex;
    private int _eyesColorIndex;
    public List<CharacterCreationSpritesClass> NoseGraphics;
    private int _noseIndex;
    private int _noseColorIndex;
    public List<CharacterCreationSpritesClass> MouthGraphics;
    private int _mouthIndex;
    private int _mouthColorIndex;

    [System.Serializable]
    public class ColorSelector
    {
        public string ColorName;
        public Color ColorValue;
    }

    public ColorSelector[] Colors;

    Image[] _characterImages;

    int _activeIndex;
    int _activeColorIndex;
    Image _activeImage;
    public List<CharacterCreationSpritesClass> ActiveParts;
    int _maxIndex;
    string _activeCategory;

    TextMeshProUGUI _partName, _colorName;

    private GameObject _rightSideSelection;
    
    private void Awake()
    {
        _characterImages = this.transform.GetChild(1).GetComponentsInChildren<Image>();
        _partName = this.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        _colorName = this.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        _rightSideSelection = this.transform.GetChild(2).GetChild(1).gameObject;
        _rightSideSelection.SetActive(false);
        SetDefaultCharacter();
    }

    public void SetGraphicsParts(List<AssetLoader_script.SpriteHolderClass> spriteHolder)
    {
        BackgroundGraphics = new List<CharacterCreationSpritesClass>();
        BodyGraphics = new List<CharacterCreationSpritesClass>();
        HeadGraphics = new List<CharacterCreationSpritesClass>();
        EyesGraphics = new List<CharacterCreationSpritesClass>();
        NoseGraphics = new List<CharacterCreationSpritesClass>();
        MouthGraphics = new List<CharacterCreationSpritesClass>();

        foreach (AssetLoader_script.SpriteHolderClass shc in spriteHolder)
        {
            string s = shc.NameString.Remove(3);
            
            CharacterCreationSpritesClass ccs = new CharacterCreationSpritesClass();
            string name;
            switch (s)
            {
                case "bg_":
                    name = shc.NameString.TrimStart('b', 'g', '_');
                    name = name.TrimEnd('.', 'p', 'n', 'g');
                    ccs.PartName = name;
                    ccs.PartSprite = shc.LoadedSprite;
                    BackgroundGraphics.Add(ccs);
                    break;
                case "bp_":
                    name = shc.NameString.TrimStart('b','p', '_');
                    name = name.TrimEnd('.', 'p', 'n', 'g');
                    ccs.PartName = name;
                    ccs.PartSprite = shc.LoadedSprite;
                    BodyGraphics.Add(ccs);
                    break;
                case "hp_":
                    name = shc.NameString.TrimStart('h', 'p', '_');
                    name = name.TrimEnd('.', 'p', 'n', 'g');
                    ccs.PartName = name;
                    ccs.PartSprite = shc.LoadedSprite;
                    HeadGraphics.Add(ccs);
                    break;
                case "ep_":
                    name = shc.NameString.TrimStart('e', 'p', '_');
                    name = name.TrimEnd('.', 'p', 'n', 'g');
                    ccs.PartName = name;
                    ccs.PartSprite = shc.LoadedSprite;
                    EyesGraphics.Add(ccs);
                    break;
                case "np_":
                    name = shc.NameString.TrimStart('n', 'p', '_');
                    name = name.TrimEnd('.', 'p', 'n', 'g');
                    ccs.PartName = name;
                    ccs.PartSprite = shc.LoadedSprite;
                    NoseGraphics.Add(ccs);
                    break;
                case "mp_":
                    name = shc.NameString.TrimStart('m', 'p', '_');
                    name = name.TrimEnd('.', 'p', 'n', 'g');
                    ccs.PartName = name;
                    ccs.PartSprite = shc.LoadedSprite;
                    MouthGraphics.Add(ccs);
                    break;
            }
        }
    }

    public void SetDefaultCharacter()
    {
        _characterImages[0].sprite = BackgroundGraphics[0].PartSprite;
        _characterImages[1].sprite = BodyGraphics[0].PartSprite;
        _characterImages[2].sprite = HeadGraphics[0].PartSprite;
        _characterImages[3].sprite = EyesGraphics[0].PartSprite;
        _characterImages[4].sprite = NoseGraphics[0].PartSprite;
        _characterImages[5].sprite = MouthGraphics[0].PartSprite;
    }

    public void SelectCategory(string name)
    {        
        switch(name)
        {
            case "Background":
                _activeIndex = _backgroundIndex;
                _maxIndex = BackgroundGraphics.Count;
                _activeColorIndex = _backgroundColorIndex;
                _activeImage = _characterImages[0];
                ActiveParts = new List<CharacterCreationSpritesClass>(BackgroundGraphics);                
                break;
            case "Body":
                _activeIndex = _bodyIndex;
                _maxIndex = BodyGraphics.Count;
                _activeColorIndex = _bodyColorIndex;
                _activeImage = _characterImages[1];
                ActiveParts = new List<CharacterCreationSpritesClass>(BodyGraphics);
                break;
            case "Head":
                _activeIndex = _headIndex;
                _maxIndex = HeadGraphics.Count;
                _activeColorIndex = _headColorIndex;
                _activeImage = _characterImages[2];
                ActiveParts = new List<CharacterCreationSpritesClass>(HeadGraphics);
                break;
            case "Eyes":
                _activeIndex = _eyesIndex;
                _maxIndex = EyesGraphics.Count;
                _activeColorIndex = _eyesColorIndex;
                _activeImage = _characterImages[3];
                ActiveParts = new List<CharacterCreationSpritesClass>(EyesGraphics);
                break;
            case "Nose":
                _activeIndex = _noseIndex;
                _maxIndex = NoseGraphics.Count;
                _activeColorIndex = _noseColorIndex;
                _activeImage = _characterImages[4];
                ActiveParts = new List<CharacterCreationSpritesClass>(NoseGraphics);
                break;
            case "Mouth":
                _activeIndex = _mouthIndex;
                _maxIndex = MouthGraphics.Count;
                _activeColorIndex = _mouthColorIndex;
                _activeImage = _characterImages[5];
                ActiveParts = new List<CharacterCreationSpritesClass>(MouthGraphics);
                break;
        }
        //Debug.Log("MaxIndex: " + _maxIndex);
        _activeCategory = name;
        _rightSideSelection.SetActive(true);
        UpdatePartInfo(_activeIndex);
        UpdateColorInfo(_activeColorIndex);
    }

    public void SelectPart(int index)
    {
        _activeIndex += index;
        if (_activeIndex > _maxIndex-1)
        {
            _activeIndex = 0;
        }
        else if (_activeIndex < 0)
        {
            _activeIndex = _maxIndex-1;
        }        
        UpdatePartInfo(_activeIndex);
        SaveIndex();
    }

    public void SelectColor(int index)
    {
        _activeColorIndex += index;
        if (_activeColorIndex > Colors.Length-1)
        {
            _activeColorIndex = 0;
        }
        else if (_activeColorIndex < 0)
        {
            _activeColorIndex = Colors.Length-1;
        }
        UpdateColorInfo(index);
        SaveIndex();
    }
        
    void UpdatePartInfo(int index)
    {
        _activeImage.sprite = ActiveParts[index].PartSprite;
        _partName.text = ActiveParts[index].PartName;                  
    }

    void UpdateColorInfo(int index)
    {        
        _colorName.text = Colors[_activeColorIndex].ColorName;
        _activeImage.color = Colors[_activeColorIndex].ColorValue;
    }

    void SaveIndex()
    {
        switch (_activeCategory)
        {
            case "Background":
                _backgroundIndex = _activeIndex;
                _backgroundColorIndex = _activeColorIndex;
                break;
            case "Body":
                _bodyIndex = _activeIndex;
                _bodyColorIndex = _activeColorIndex;
                break;
            case "Head":
                _headIndex = _activeIndex;
                _headColorIndex = _activeColorIndex;
                break;
            case "Eyes":
                _eyesIndex = _activeIndex;
                _eyesColorIndex = _activeColorIndex;
                break;
            case "Nose":
                _noseIndex = _activeIndex;
                _noseColorIndex = _activeColorIndex;
                break;
            case "Mouth":
                _mouthIndex = _activeIndex;
                _mouthColorIndex = _activeColorIndex;
                break;
            default:
                Debug.Log("Invalid caterogry " + _activeCategory);
                break;
        }
    }

    public void GetIndexValues(out List<int> partIndex, out List<int> colorIndex)
    {
        Debug.Log("Get Avatar index");
        partIndex = new List<int>();
        colorIndex = new List<int>();

        partIndex.Add(_backgroundIndex);
        colorIndex.Add(_backgroundColorIndex);

        partIndex.Add(_bodyIndex);
        colorIndex.Add(_bodyColorIndex);

        partIndex.Add(_headIndex);
        colorIndex.Add(_headColorIndex);

        partIndex.Add(_eyesIndex);
        colorIndex.Add(_eyesColorIndex);

        partIndex.Add(_noseIndex);
        colorIndex.Add(_noseColorIndex);

        partIndex.Add(_mouthIndex);
        colorIndex.Add(_mouthColorIndex);

    }

    public void GetAvatarInfo(List<int> partIndex, List<int> colorIndex, out List<Sprite> avatarSprites, out List<Color> avatarColors)
    {
        avatarSprites = new List<Sprite>();
        avatarColors = new List<Color>();

        avatarSprites.Add(BackgroundGraphics[partIndex[0]].PartSprite);
        avatarColors.Add(Colors[colorIndex[0]].ColorValue);

        avatarSprites.Add(BodyGraphics[partIndex[1]].PartSprite);
        avatarColors.Add(Colors[colorIndex[1]].ColorValue);

        avatarSprites.Add(HeadGraphics[partIndex[2]].PartSprite);
        avatarColors.Add(Colors[colorIndex[2]].ColorValue);

        avatarSprites.Add(EyesGraphics[partIndex[3]].PartSprite);
        avatarColors.Add(Colors[colorIndex[3]].ColorValue);

        avatarSprites.Add(NoseGraphics[partIndex[4]].PartSprite);
        avatarColors.Add(Colors[colorIndex[4]].ColorValue);

        avatarSprites.Add(MouthGraphics[partIndex[5]].PartSprite);
        avatarColors.Add(Colors[colorIndex[5]].ColorValue);
    }
}
