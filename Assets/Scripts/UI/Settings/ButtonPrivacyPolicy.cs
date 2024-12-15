//#region import
using System.Collections;
using System.Collections.Generic;
using Lofelt.NiceVibrations;
using UnityEngine;
//#endregion

public class ButtonPrivacyPolicy : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private string IOS_URL = "https://www.iubenda.com/privacy-policy/68611982";
    [SerializeField] private string Android_URL = "https://docs.google.com/document/d/11Z0z1jL3egPpPAF5L3S8r5CKpCvFpFNTPtjqnVStMho/edit";
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks
    //#endregion

    //#region public methods

    public void Click()
    {
        GameEventManager.PlayVibration?.Invoke(HapticPatterns.PresetType.Selection);

#if UNITY_EDITOR
        Application.OpenURL(Android_URL);
        Application.OpenURL(IOS_URL);
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.OpenURL(Android_URL);
#endif
#if UNITY_IOS
        Application.OpenURL(IOS_URL);
#endif
    }

    //#endregion

    //#region private methods
    //#endregion

    //#region event handlers
    //#endregion
}
