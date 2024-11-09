using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject lockObject;

    public Button Button
    {
        get { return GetComponent<Button>(); }
        private set { }
    }

    public void Configure(Sprite sprite, bool unlocked)
    {
        characterImage.sprite = sprite;
        if (unlocked)
        {
            UnLock();
        }
        else
        {
            Lock();
        }
    }

    public void Lock()
    {
        lockObject.SetActive(true);
        characterImage.color = Color.gray;
    }

    public void UnLock()
    {
        lockObject.SetActive(false);
        characterImage.color = Color.white;
    }


}
