﻿using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.model;
using UnityEngine;
using UnityEngine.UI;

public class CardGameObject : MonoBehaviour {
    public int Id;
    public String UUID;
    public string Name;
    public string Description;
    private List<Resource> Costs;

    public void Initialize(int id, string uuid, string name, string description, List<Resource> costs)
    {
        Id = id;
        Name = name;
        Description = description;
        Costs = costs;
        UUID = uuid;
    }

    void Start()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        texts[0].text = Name;
        texts[1].text = Description;
    }
}
