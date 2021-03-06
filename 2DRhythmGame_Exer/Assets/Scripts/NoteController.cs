using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NoteController : MonoBehaviour
{
    // Start is called before the first frame update

    class Note
    {
        public int noteType { get; set; }
        public int order { get; set; }
        public Note(int noteType, int order)
        {
            this.noteType = noteType;
            this.order = order;
        }
    }//end of class : Note


    public GameObject[] Notes;

    private ObjectPooler noteObjectPooler;
    private List<Note> notes = new List<Note>();

    private float x, z, startY = 8.0f;
 


    private string musicTitle;
    private string musicArtist;
    private int bpm;
    private int divider;
    private float startingPoint;
    private float beatCount;
    private float beatInterval;


    void MakeNote(Note note)
    {
        GameObject obj = noteObjectPooler.getObject(note.noteType);

        //설정된 시작 라인으로 노트를 이동시킵니다.
        x = obj.transform.position.x;
        z = obj.transform.position.z;

        obj.transform.position = new Vector3(x, startY, z);
        obj.GetComponent<NoteBehavior>().Initialize();
        obj.SetActive(true);
    }

    IEnumerator AwaitMakeNote(Note note)
    {
        int noteType = note.noteType;
        int order = note.order;
        yield return new WaitForSeconds(startingPoint + order * beatInterval);
        MakeNote(note);
    }

    void Start()
    {
        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();

        // 리소스(text)에서 비트정보를 가져온다

        TextAsset textAsset = Resources.Load<TextAsset>("Beats/"+GameManager.instance.music);
        StringReader reader = new StringReader(textAsset.text);
        musicTitle = reader.ReadLine();
        musicArtist = reader.ReadLine();

        string beatInformation = reader.ReadLine();
        bpm = Convert.ToInt32(beatInformation.Split(' ')[0]);//Split()함수를 가져와서 공백을 기준으로 나눈다.
        divider = Convert.ToInt32(beatInformation.Split(' ')[1]);
        startingPoint = (float)Convert.ToDouble(beatInformation.Split(' ')[2]);


        beatCount = (float)bpm / divider;//1초마다 떨어지는 비트 개수
        beatInterval = 1 / beatCount; //비트가 떨어지는 간격시간

        //각 비트들이 떨어지는 위치 및 시간정보를 읽는다
        string line;
        while((line= reader.ReadLine())!= null)
        {
            Note note = new Note(
                Convert.ToInt32(line.Split(' ')[0]) + 1,
                Convert.ToInt32(line.Split(' ')[1])
                ) ;
            notes.Add(note);

        }

        //모든 노트를 정해진 시간에 출발하도록 설정
        for (int i = 0; i < notes.Count; i++)
        {
            StartCoroutine(AwaitMakeNote(notes[i]));
        }

        //마지막 노트를 기준으로 게임 종료 함수를 불러옵니다
        StartCoroutine(AwaitGameResult(notes[notes.Count - 1].order));



    }
    IEnumerator AwaitGameResult(int order)
    {
        yield return new WaitForSeconds(startingPoint + order * beatInterval + 3.0f);
        GameResult();

    }
    void GameResult()
    {
        SceneManager.LoadScene("GameResultScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
