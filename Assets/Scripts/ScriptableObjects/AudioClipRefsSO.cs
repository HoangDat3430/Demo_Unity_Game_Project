using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] chop;
    public AudioClip[] deliverySuccess;
    public AudioClip[] deliveryFail;
    public AudioClip[] footStep;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickup;
    public AudioClip   stoveSizzle;
    public AudioClip[] countdown;
    public AudioClip[] start;
    public AudioClip[] warning;
    public AudioClip[] trash;
    public AudioClip[] gamePassed;
    public AudioClip[] gameUnpassed;
    public AudioClip[] triggerTrap;
    public AudioClip[] tenMark;
}
