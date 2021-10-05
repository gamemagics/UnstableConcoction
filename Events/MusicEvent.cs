using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicEvent : UnityEvent<string> {}

public class MusicEventManager {
    public static MusicEvent musicEvent = new MusicEvent();
}
