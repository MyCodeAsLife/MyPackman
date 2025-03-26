﻿using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "ApplicationSettings", menuName = "Game Settings/new Application Settings")]
    public class ApplicationSettings : ScriptableObject
    {
        public int MusicVolume;
        public int SFXVolume;
        public string Difficulty;
    }
}
