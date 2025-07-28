using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevNote
{
    public class ServiceTestWindowView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _versionText;

        [Header("Environment:")]
        [SerializeField] private TextMeshProUGUI _environmentSelectedServiceText;
        [SerializeField] private TextMeshProUGUI _environmentTestEnabledText;
        [SerializeField] private TextMeshProUGUI _environmentSelectedTypeText;
        [SerializeField] private TextMeshProUGUI _environmentLanguageText;
        [SerializeField] private TextMeshProUGUI _environmentDeviceTypeText;

        [Header("Ads:")]
        [SerializeField] private TextMeshProUGUI _adsSelectedServiceText;
        [SerializeField] private Button _adsShowRewardedButton;
        [SerializeField] private Button _adsShowInterstitialButton;
        [SerializeField] private Button _adsEnableBannerButton;
        [SerializeField] private Button _adsDisableBannerButton;

        [Header("Saves:")]
        [SerializeField] private TextMeshProUGUI _savesSelectedServiceText;
        [SerializeField] private Button _savesSaveLocalButton;
        [SerializeField] private Button _savesSaveCloudButton;

        [Header("Purchases:")]
        [SerializeField] private ProductType _testProductKey;
        [SerializeField] private TextMeshProUGUI _purchasesSelectedServiceText;
        [SerializeField] private TextMeshProUGUI _purchasesProductKeyText;
        [SerializeField] private TextMeshProUGUI _purchasesProductPriceText;
        [SerializeField] private Button _purchasesPurchaseButton;

        [Header("Analytics:")]
        [SerializeField] private TextMeshProUGUI _analyticsSelectedServiceText;
        [SerializeField] private Button _analyticsSendTestEventButton;

        [Header("Review:")]
        [SerializeField] private TextMeshProUGUI _reviewSelectedServiceText;
        [SerializeField] private Button _reviewRequestButton;


        private readonly Holder<IEnvironment> environment = new();
        private readonly Holder<IAds> ads = new();
        private readonly Holder<ISave> save = new();
        private readonly Holder<IPurchase> purchase = new();
        private readonly Holder<IAnalytics> analytics = new();
        private readonly Holder<IReview> review = new();


        private readonly Color successColor = new Color(0.43f, 1f, 0.45f, 1f);
        private readonly Color errorColor = new Color(1f, 0.43f, 0.48f, 1f);
        private readonly Color pendingColor = new Color(1f, 1f, 0.7f, 1f);



        private void Start()
        {
            _adsShowRewardedButton.onClick.AddListener(OnShowRewardedButtonClick);
            _adsShowInterstitialButton.onClick.AddListener(OnShowInterstitialButtonClick);
            _adsEnableBannerButton.onClick.AddListener(OnEnableBannerButtonClick);
            _adsDisableBannerButton.onClick.AddListener(OnDisableBannerButtonClick);
            _savesSaveLocalButton.onClick.AddListener(OnSaveLocalButtonClick);
            _savesSaveCloudButton.onClick.AddListener(OnSaveCloudButtonClick);
            _purchasesPurchaseButton.onClick.AddListener(OnPurchaseButtonClick);
            _analyticsSendTestEventButton.onClick.AddListener(OnSendTestEventButtonClick);
            _reviewRequestButton.onClick.AddListener(OnReviewButtonClick);

            Display();
        }

        private void Display()
        {
            _versionText.text = Const.VERSION;

            _environmentSelectedServiceText.text = environment.Value.GetType().Name.Replace("EnvironmentService", string.Empty);
            _adsSelectedServiceText.text = ads.Value.GetType().Name.Replace("AdsService", string.Empty);
            _savesSelectedServiceText.text = save.Value.GetType().Name.Replace("SaveService", string.Empty);
            _purchasesSelectedServiceText.text = purchase.Value.GetType().Name.Replace("PurchaseService", string.Empty);
            _analyticsSelectedServiceText.text = analytics.Value.GetType().Name.Replace("AnalyticsService", string.Empty);
            _reviewSelectedServiceText.text = review.Value.GetType().Name.Replace("ReviewService", string.Empty);

            string testValue = IEnvironment.IsTest ? "Active" : "Disabled";
            _environmentTestEnabledText.text = _environmentTestEnabledText.text.Replace("<test>", testValue);

            string environmentTypeValue = IEnvironment.EnvironmentType.ToString();
            _environmentSelectedTypeText.text = _environmentSelectedTypeText.text.Replace("<type>", environmentTypeValue);

            string languageValue = environment.Value.CurrentLanguage.ToString();
            _environmentLanguageText.text = _environmentLanguageText.text.Replace("<language>", languageValue);

            string controlValue = environment.Value.DeviceType.ToString();
            _environmentDeviceTypeText.text = _environmentDeviceTypeText.text.Replace("<device>", controlValue);

            string priceValue = purchase.Value.GetPriceString(_testProductKey);
            _purchasesProductPriceText.text = _purchasesProductPriceText.text.Replace("<price>", priceValue);
            _purchasesProductKeyText.text = _purchasesProductKeyText.text.Replace("<key>", _testProductKey.ToString());

        }


        private void OnDisableBannerButtonClick() => ads.Value.SetBanner(false);
        private void OnEnableBannerButtonClick() => ads.Value.SetBanner(true);

        private void OnReviewButtonClick() => review.Value.Request();

        private void OnSendTestEventButtonClick() => analytics.Value.SendEvent("test_event", new Dictionary<string, object>()
        {
            { "random_int" , Random.Range(0, 3) },
            { "device_type" , environment.Value.DeviceType.ToString() },
        });

        private void OnPurchaseButtonClick()
        {
            _purchasesPurchaseButton.image.color = pendingColor;

            purchase.Value.Purchase(_testProductKey,
                onSuccess: () => _purchasesPurchaseButton.image.color = successColor,
                onError: () => _purchasesPurchaseButton.image.color = errorColor);
        }

        private void OnSaveCloudButtonClick()
        {
            _savesSaveCloudButton.image.color = pendingColor;

            save.Value.SaveCloud(
                onSuccess: () => _savesSaveCloudButton.image.color = successColor,
                onError: () => _savesSaveCloudButton.image.color = errorColor);
        }

        private void OnSaveLocalButtonClick()
        {
            _savesSaveLocalButton.image.color = pendingColor;

            save.Value.SaveLocal(
                onSuccess: () => _savesSaveLocalButton.image.color = successColor,
                onError: () => _savesSaveLocalButton.image.color = errorColor);
        }



        private void OnShowInterstitialButtonClick()
        {
            _adsShowInterstitialButton.image.color = pendingColor;

            ads.Value.ShowInterstitial(AdKey.None,
                onShown: () => _adsShowInterstitialButton.image.color = successColor,
                onError: () => _adsShowInterstitialButton.image.color = errorColor);
        }

        private void OnShowRewardedButtonClick()
        {
            _adsShowRewardedButton.image.color = pendingColor;

            ads.Value.ShowRewarded(AdKey.None,
                onRewarded: () => _adsShowRewardedButton.image.color = successColor,
                onError: () => _adsShowRewardedButton.image.color = errorColor);
        }


        



    }
}

