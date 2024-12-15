using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/GlobalSettings", order = 1)]
public class GlobalSettingsScriptableObject : ScriptableObject
{
    [BoxGroup("Transfer")]
    public float durationTransferCell = 0.2f;
    [BoxGroup("Transfer")]
    public Ease easingTransferCell = Ease.InOutSine;
}