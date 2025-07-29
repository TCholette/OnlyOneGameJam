using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;


public static class ProxyFmodPlayer 
{
    const string EVENT_DIRECTORY = "event:/";

    public static EventInstance? PlaySound<T>(string sound, GameObject gameObject, KeyValuePair<string, T>? parameterPairs = null) {
        EventInstance soundEvent = (EventInstance)CreateSound(sound, gameObject, parameterPairs);
        soundEvent.start();
        return soundEvent;
    }

    public static EventInstance? CreateSound<T>(string sound, GameObject gameObject, KeyValuePair<string, T>? parameterPairs = null) {
        if (!EventManager.IsInitialized) {
            return null;
        }
        EventInstance soundEvent = RuntimeManager.CreateInstance(EventReference.Find(EVENT_DIRECTORY + sound));
        SetParam(soundEvent, parameterPairs);
        RuntimeManager.AttachInstanceToGameObject(soundEvent, gameObject);
        return soundEvent;
    }

    public static void SetParam<T>(EventInstance soundEvent, KeyValuePair<string, T>? parameterPairs = null) {
        if (parameterPairs != null) {
            if (typeof(T) == typeof(string)) {
                soundEvent.setParameterByNameWithLabel(parameterPairs.Value.Key, parameterPairs.Value.Value.ToString());
            }
            else {
                soundEvent.setParameterByName(parameterPairs.Value.Key, Convert.ToSingle(parameterPairs.Value.Value));
            }
        }
    }

    public static void DisableSound() {
        Debug.Log("SHOULD DISABLE SOUND");
        var vca = RuntimeManager.GetVCA("vca:/Master");
        vca.setVolume(0);
    }

    public static void EnableSound() {
        Debug.Log("SHOULD ENABLE SOUND");
        var vca = RuntimeManager.GetVCA("vca:/Master");
        vca.setVolume(1);
    }
}