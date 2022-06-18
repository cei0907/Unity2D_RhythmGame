using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour
{

    public GameObject gameBackground;
    private SpriteRenderer gameBackgroundSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        gameBackgroundSpriteRenderer = gameBackground.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(gameBackgroundSpriteRenderer, 0.005f));
        
    }

    IEnumerator FadeOut(SpriteRenderer spriteRenderer, float amount)
    {
        Color color = spriteRenderer.color;
        while(color.a > 0.0f)
        {
            color.a -= amount;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(amount);//0.005f만큼 쉬었다가 다시 실행하게 해주는 것.. 이것이 yield 키워드인지 조사해보자
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
