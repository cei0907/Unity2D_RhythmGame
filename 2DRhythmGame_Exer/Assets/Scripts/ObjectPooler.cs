﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //이중 리스트 사용하기
    //Note 1 : 10개 -> 리스트
    //Note 2 : 10개 -> 리스트
    //Note 3 : 10개 -> 리스트
    //Note 4 : 10개 -> 리스트
    // Start is called before the first frame update

    //미리 많은 오프젝트를 만들어놨다가 그것들을 돌려쓰는 방법

    public List<GameObject> Notes;
    private List<List<GameObject>> poolsOfNotes;

    public int noteCount = 10;
    private bool more = true; 
    void Start()
    {
        poolsOfNotes = new List<List<GameObject>>();

        for(int i=0; i<Notes.Count; i++)
        {
            poolsOfNotes.Add(new List<GameObject>());
            for(int n=0; n<noteCount; n++) {
                GameObject obj = Instantiate(Notes[i]);
                obj.SetActive(false);
                poolsOfNotes[i].Add(obj); 
            }
        }
    }

    public GameObject getObject(int noteType)
    {
        foreach(GameObject obj in poolsOfNotes[noteType - 1]){
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        if (more)
        {
            GameObject obj = Instantiate(Notes[noteType - 1]);
            poolsOfNotes[noteType - 1].Add(obj);
            return obj;
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
