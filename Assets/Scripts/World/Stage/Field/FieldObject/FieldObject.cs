﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//필드 내에서 존재하는 모든 오브젝트들 
public abstract class FieldObject : MonoBehaviour
{
    public Vector3 Position { get; protected set; }
    public float Radius { get; protected set; }
}