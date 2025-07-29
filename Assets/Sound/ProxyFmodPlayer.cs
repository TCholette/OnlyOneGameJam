using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;


public static class ProxyFmodPlayer 
{
    const string EVENT_DIRECTORY = "event:/";

    public static EventInstance? PlaySound<T>(string sound, GameObject gameObject, KeyValuePair<string, T>? parameterPairs = null) {
        if (!EventManager.IsInitialized) {
            return null;
        }
        EventInstance soundEvent = RuntimeManager.CreateInstance(EventReference.Find(EVENT_DIRECTORY + sound));
        RuntimeManager.AttachInstanceToGameObject(soundEvent, gameObject);
        if (parameterPairs != null) {
            if (typeof(T) == typeof(string)) {
                soundEvent.setParameterByNameWithLabel(parameterPairs.Value.Key, parameterPairs.Value.Value.ToString());
            }
            else {
                soundEvent.setParameterByName(parameterPairs.Value.Key, Convert.ToSingle(parameterPairs.Value.Value));
            }
        }
        soundEvent.start();
        return soundEvent;
    }
}