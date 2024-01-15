using UnityEngine;
using UnityEngine.Events;

public class ChipUnificationEvent : UnityEvent <int, Vector3, eChipColors> { }
public class EventWithPosition : UnityEvent <Vector3> { }
public class FillingSkillEvent : UnityEvent<int, eChipColors> { }
public class FullSkillEvent : UnityEvent<eChipColors> { }
public class ChangeVolumeEvent : UnityEvent<float> { }
public class DestroyEffectEvent : UnityEvent<Vector3, eChipColors> { }
public class EventWithInt : UnityEvent<int> { }

public class EventAggregator : MonoBehaviour
{
    /// <summary>
    /// Parameters: value, position, color, with sound
    /// </summary>
    public static ChipUnificationEvent ChipUnification = new ChipUnificationEvent();
    public static EventWithPosition DestroyCristal = new EventWithPosition();
    public static EventWithPosition DestroyFire = new EventWithPosition();
    public static EventWithPosition DestroyFrost = new EventWithPosition();
    /// <summary>
    /// Parameters: value, color
    /// </summary>
    public static FillingSkillEvent IncreaseSkillFilling = new FillingSkillEvent();
    /// <summary>
    /// Parameters: color
    /// </summary>
    public static FullSkillEvent FullSkill = new FullSkillEvent();
    public static ChangeVolumeEvent SetMusicVolume = new ChangeVolumeEvent();
    public static ChangeVolumeEvent SetSoundsVolume = new ChangeVolumeEvent();
    /// <summary>
    /// Parameters: position, color
    /// </summary>
    public static DestroyEffectEvent ShowDestroyEffect = new DestroyEffectEvent();
    /// <summary>
    /// Parameters: stage
    /// </summary>
    public static UnityEvent NewStage = new UnityEvent();
    public static UnityEvent UnificationSound = new UnityEvent();
    public static UnityEvent AddNewSkill = new UnityEvent();
    public static UnityEvent UseLightning = new UnityEvent();
    public static UnityEvent ThrowChip = new UnityEvent();
    public static UnityEvent Lose = new UnityEvent();
    public static UnityEvent SetRussianLanguage = new UnityEvent();
    public static FullSkillEvent SkillFilled = new FullSkillEvent();
    public static UnityEvent StopPlaySounds = new UnityEvent();
    public static UnityEvent ContinuePlaySounds = new UnityEvent();
    public static UnityEvent Init = new UnityEvent();
}
