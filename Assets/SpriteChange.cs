﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class SpriteChange : MonoBehaviour
{
    public Sprite[] sprites;
    [SerializeField]
    private string playerPref;
    private SpriteRenderer sr;



    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[PlayerPrefs.GetInt(playerPref)];
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
