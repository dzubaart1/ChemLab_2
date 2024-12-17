using System;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class KeyButtonComponent : ButtonComponent
    {
        [SerializeField] private int _value;

        public int Value
        {
            get { return _value; }
        }
    }
}