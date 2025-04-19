using UnityEngine;

public class PixelText : MonoBehaviour
{
    public Sprite[] letterSprites; 
    public string textToDisplay = "SCORE 100";
    public GameObject letterPrefab; 

    void Start()
    {
        DisplayText();
    }

    void DisplayText()
    {
        float spacing = 0.2f; 

        for (int i = 0; i < textToDisplay.Length; i++)
        {
            char c = char.ToUpper(textToDisplay[i]);

            int index = GetSpriteIndex(c);
            if (index == -1) continue;

            GameObject letterObj = Instantiate(letterPrefab, transform);
            letterObj.transform.localPosition = new Vector3(i * spacing, 0, 0);
            letterObj.GetComponent<SpriteRenderer>().sprite = letterSprites[index];
        }
    }

    int GetSpriteIndex(char c)
    {
        if (c >= 'A' && c <= 'Z') return c - 'A';
        if (c >= '0' && c <= '9') return 26 + (c - '0'); 
        return -1;
    }
}


