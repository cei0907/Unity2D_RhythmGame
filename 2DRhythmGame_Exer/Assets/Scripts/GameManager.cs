using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; set; }



    public float noteSpeed;

    public GameObject scoreUI;
    private float score;
    private Text scoreText;

    public GameObject comboUI;
    private float combo;
    private Text comboText;
    private Animator comboAnimator;

    public enum judges { NONE = 0, BAD, GOOD, PERFECT, MISS };

    public GameObject judgeUI;
    private Sprite[] judgeSprites;
    private Image judgementSpriteRenderer;
    private Animator judgementSpriteAnimator;

    public GameObject[] trails;
    private SpriteRenderer[] trailSpriteRenderers;
 
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }



    // Start is called before the first frame update
    void Start()
    {
        judgementSpriteRenderer = judgeUI.GetComponent<Image>();
        judgementSpriteAnimator = judgeUI.GetComponent<Animator>();

        scoreText = scoreUI.GetComponent<Text>();
        comboText = comboUI.GetComponent<Text>();

        comboAnimator = comboUI.GetComponent<Animator>();

        //판정 결과를 보여주는 스프라이트 이미지를 미리 초기화합니다.
        judgeSprites = new Sprite[4];
        judgeSprites[0] =   Resources.Load<Sprite>("Sprites/Bad");
        judgeSprites[1] =   Resources.Load<Sprite>("Sprites/Good");
        judgeSprites[2] =   Resources.Load<Sprite>("Sprites/Miss");
        judgeSprites[3] =   Resources.Load<Sprite>("Sprites/Perfect");

        trailSpriteRenderers = new SpriteRenderer[trails.Length];
        for(int i=0; i<trails.Length; i++)
        {
            trailSpriteRenderers[i] = trails[i].GetComponent<SpriteRenderer>();    
        }
    }

    // Update is called once per frame
    void Update()
    {
        //사용자가 입력한 키에 해당하는 라인을 빛나게 처리합니다.
        if (Input.GetKey(KeyCode.D))
        {
            ShineTrail(0);
        }
        if (Input.GetKey(KeyCode.F))
        {
            ShineTrail(1);
        }
        if (Input.GetKey(KeyCode.J))
        {
            ShineTrail(2);
        }
        if (Input.GetKey(KeyCode.K))
        {
            ShineTrail(3);
        }
        for(int i=0; i<trailSpriteRenderers.Length; i++)
        {
            Color color = trailSpriteRenderers[i].color;
            color.a -= 0.01f;
            trailSpriteRenderers[i].color = color;
        }
    }
    public void ShineTrail(int index)
    {
        Color color = trailSpriteRenderers[index].color;
        color.a = 0.32f;
        trailSpriteRenderers[index].color = color;
    }

    // 노트 판정 이후에 판정 결과를 화면에 보여줍니다

    void showJudgement()
    {
        string scoreFormat = "000000";//6자리로 나올 수 있도록 format을 만들어줬다.
        scoreText.text = score.ToString(scoreFormat); //실제 점수를 scoreFormat형태의 string으로 바꿔주는 것

        //판정 이미지를 보여줍니다.
        judgementSpriteAnimator.SetTrigger("Show");

        if (combo > 1)
        {
            comboText.text = "COMBO " + combo.ToString();
            comboAnimator.SetTrigger("Show");
        }

    }

    //노트 판정을 진행합니다.
    public void processJudge(judges judge, int noteType)
    {
        if (judge == judges.NONE) return;

        if (judge == judges.MISS)
        {
            judgementSpriteRenderer.sprite = judgeSprites[2];
            combo = 0;
            if (score >= 15) score -= 15;
            else score = 0;

        }
        else if(judge == judges.BAD)
        {
            judgementSpriteRenderer.sprite = judgeSprites[0];
            combo = 0;
            if (score >= 5) score -= 5;
            else score = 0;
        }
        else
        {
            if (judge == judges.PERFECT)
            {
                judgementSpriteRenderer.sprite = judgeSprites[3];
                score += 20;
            }
            else if(judge == judges.GOOD)
            {
                judgementSpriteRenderer.sprite = judgeSprites[1];
                score += 10;
            }
            combo += 1;
            score += (float)combo * 0.1f;

        }//end of else
        showJudgement();
    }
}
