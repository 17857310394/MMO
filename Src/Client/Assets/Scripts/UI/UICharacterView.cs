using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour
{
    public GameObject[] characters;

    int currentCharacter = 0;

    public GameObject CharacterRoot;

    public int CurrentCharacter
    {
        get
        {
            return currentCharacter;
        }
        set
        {
            currentCharacter = value;
            UpdateCharacter();

        }
    }
    void UpdateCharacter()
    {
        for (int i = 0; i < 3; i++)
        {
            characters[i].SetActive(currentCharacter==i);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxis("Mouse X");
            CharacterRoot.transform.Rotate(0, -x, 0);
        }
    }
}
