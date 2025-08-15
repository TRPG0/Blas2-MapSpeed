using BlasII.ModdingAPI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.InputSystem;
using Il2CppTGK.UI;
using MelonLoader;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MapSpeed
{
    public class MapSpeedLogic : MonoBehaviour
    {
        public static MapSpeedLogic Instance { get; private set; }

        public InputData inputFaster;
        public InputData inputSlower;

        public UIPixelTextWithShadow kbmSpeedText;

        public UIPixelTextWithShadow kbmFasterText;
        public UIButtonIcon kbmFasterIcon;

        public UIPixelTextWithShadow kbmSlowerText;
        public UIButtonIcon kbmSlowerIcon;

        public UIPixelTextWithShadow padSpeedText;

        public UIPixelTextWithShadow padFasterText;
        public UIButtonIcon padFasterIcon;

        public UIPixelTextWithShadow padSlowerText;
        public UIButtonIcon padSlowerIcon;

        public static readonly Color textWhite = new Color(0.972f, 0.894f, 0.776f);
        public static readonly Color textRed = new Color(0.678f, 0.098f, 0.098f);
        private float timer = 0;

        public void Awake()
        {
            if (Instance != null && Instance != this) return;
            Instance = this;
        }

        public void Setup()
        {
            MapWindowLogic mwl = GetComponent<MapWindowLogic>();
            
            GameObject kbmLegend = mwl.legendNormal.transform.Find("kb legend/").gameObject;
            kbmLegend.transform.localPosition = new Vector3(-70, 440.2f, 0);
            GameObject kbmCenter = kbmLegend.transform.Find("Center KB/").gameObject;
            GameObject kbmSpeed = GameObject.Instantiate(kbmCenter, kbmCenter.transform.parent);
            kbmSpeed.name = "Speed KB";
            GameObject.Destroy(kbmSpeed.transform.Find("image/").gameObject);
            Component.Destroy(kbmSpeed.GetComponent<UIButtonIcon>());
            Component.Destroy(kbmSpeed.GetComponent<HorizontalLayoutGroup>());
            kbmSpeedText = kbmSpeed.GetComponent<UIButtonWithText>().text;
            kbmSpeed.transform.Find("label/").localPosition = new Vector3(65, -35, 0);

            GameObject kbmFaster = GameObject.Instantiate(kbmCenter, kbmCenter.transform.parent);
            kbmFaster.name = "Faster KB";
            kbmFasterText = kbmFaster.GetComponent<UIButtonWithText>().text;
            kbmFasterIcon = kbmFaster.GetComponent<UIButtonIcon>();

            GameObject kbmSlower = GameObject.Instantiate(kbmCenter, kbmCenter.transform.parent);
            kbmSlower.name = "Slower KB";
            kbmSlowerText = kbmSlower.GetComponent<UIButtonWithText>().text;
            kbmSlowerIcon = kbmSlower.GetComponent<UIButtonIcon>();

            kbmSlower.transform.SetAsFirstSibling();
            kbmFaster.transform.SetAsFirstSibling();
            kbmSpeed.transform.SetAsFirstSibling();

            GameObject padLegend = mwl.legendNormal.transform.Find("controller legend/").gameObject;
            padLegend.transform.localPosition = new Vector3(0, 340, 0);
            GameObject padCenter = padLegend.transform.Find("Center/").gameObject;
            GameObject padSpeed = GameObject.Instantiate(padCenter, padCenter.transform.parent);
            padSpeed.name = "Speed";
            GameObject.Destroy(padSpeed.transform.Find("image/").gameObject);
            Component.Destroy(padSpeed.GetComponent<UIButtonIcon>());
            Component.Destroy(padSpeed.GetComponent<HorizontalLayoutGroup>());
            padSpeedText = padSpeed.GetComponent<UIButtonWithText>().text;
            padSpeed.transform.Find("label/").localPosition = new Vector3(65, -35, 0);

            GameObject padFaster = GameObject.Instantiate(padCenter, padCenter.transform.parent);
            padFaster.name = "Faster";
            padFasterText = padFaster.GetComponent<UIButtonWithText>().text;
            padFasterIcon = padFaster.GetComponent<UIButtonIcon>();

            GameObject padSlower = GameObject.Instantiate(padCenter, padCenter.transform.parent);
            padSlower.name = "Slower";
            padSlowerText = padSlower.GetComponent<UIButtonWithText>().text;
            padSlowerIcon = padSlower.GetComponent<UIButtonIcon>();

            padSlower.transform.SetAsFirstSibling();
            padFaster.transform.SetAsFirstSibling();
            padSpeed.transform.SetAsFirstSibling();

            kbmLegend.SetActive(true);
            padLegend.SetActive(true);

            mwl.legendNormal.GetComponent<ShowWithInputDevice>().InputDeviceManager_OnMainDeviceChanged();

            inputSlower = CoreCache.Input.GetCurrentDefinedInputs().FindInput("UI Shoulder Left 2");
            inputFaster = CoreCache.Input.GetCurrentDefinedInputs().FindInput("UI Shoulder Right 2");

            kbmFasterIcon.input = inputFaster;
            kbmFasterIcon.Refresh();

            kbmSlowerIcon.input = inputSlower;
            kbmSlowerIcon.Refresh();

            padFasterIcon.input = inputFaster;
            padFasterIcon.Refresh();

            padSlowerIcon.input = inputSlower;
            padSlowerIcon.Refresh();
        }

        public void OnEnable()
        {
            MapWindowLogic mwl = GetComponent<MapWindowLogic>();
            mwl.legendNormal.transform.Find("kb legend/").gameObject.SetActive(true);
            mwl.legendNormal.transform.Find("controller legend/").gameObject.SetActive(true);
            mwl.legendNormal.GetComponent<ShowWithInputDevice>().InputDeviceManager_OnMainDeviceChanged();
            MelonCoroutines.Start(SetText());
        }

        public IEnumerator SetText()
        {
            yield return null;
            SetTextNow();
        }

        public void SetTextNow()
        {
            kbmSpeedText.SetText(Main.MapSpeed.LocalizationHandler.Localize("speed").Replace("*", MapSpeed.Speed.ToString("F")));
            padSpeedText.SetText(Main.MapSpeed.LocalizationHandler.Localize("speed").Replace("*", MapSpeed.Speed.ToString("F")));

            kbmFasterText.SetText(Main.MapSpeed.LocalizationHandler.Localize("speedup"));
            kbmSlowerText.SetText(Main.MapSpeed.LocalizationHandler.Localize("speeddown"));

            padFasterText.SetText(Main.MapSpeed.LocalizationHandler.Localize("speedup"));
            padSlowerText.SetText(Main.MapSpeed.LocalizationHandler.Localize("speeddown"));
        }

        public void Update()
        {
            if (CoreCache.Input.IsInputDown(inputFaster)) SpeedUp();
            if (CoreCache.Input.IsInputDown(inputSlower)) SpeedDown();
            
            if (timer > 0f)
            {
                Color color = Color.Lerp(textWhite, textRed, timer);
                kbmSpeedText.normalText.color = color;
                padSpeedText.normalText.color = color;
                timer -= Time.deltaTime * 2;
            }

            if (timer < 0f)
            {
                timer = 0f;
            }
        }

        public void PlaySound()
        {
            // couldn't really figure out how to play a sound. this is very stupid
            transform.Find("Events/ZoomIn/").GetComponent<UIGlobalEventBroadcaster>().BroadcastEvent();
        }

        public void SpeedUp()
        {
            if (MapSpeed.Speed < 3f)
            {
                MapSpeed.Speed += 0.5f;
                SetTextNow();
            }
            else timer = 1.5f;
            PlaySound();
        }

        public void SpeedDown()
        {
            if (MapSpeed.Speed > 0.5f)
            {
                MapSpeed.Speed -= 0.5f;
                SetTextNow();
            }
            else timer = 1.5f;
            PlaySound();
        }
    }
}
