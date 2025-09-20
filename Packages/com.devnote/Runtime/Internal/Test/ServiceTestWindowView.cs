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
        [SerializeField] protected TextMeshProUGUI _usedSaveTimeText;
        [SerializeField] private Button _adsShowRewardedButton;
        [SerializeField] private Button _adsShowInterstitialButton;
        [SerializeField] private Button _adsEnableBannerButton;
        [SerializeField] private Button _adsDisableBannerButton;

        [Header("Saves:")]
        [SerializeField] private TextMeshProUGUI _savesSelectedServiceText;
        [SerializeField] private Button _savesSaveLocalButton;
        [SerializeField] private Button _savesSaveCloudButton;

        [Header("Purchases:")]
        [SerializeField] private TextMeshProUGUI _purchasesSelectedServiceText;
        [SerializeField] private TextMeshProUGUI _purchasesProductKeyText;
        [SerializeField] private TextMeshProUGUI _purchasesProductPriceText;
        [SerializeField] private Button _purchasesPurchaseButton;
        [SerializeField] private TextMeshProUGUI _purchaseButtonText;

        [Header("Analytics:")]
        [SerializeField] private TextMeshProUGUI _analyticsSelectedServiceText;
        [SerializeField] private Button _analyticsSendTestEventButton;

        [Header("Review:")]
        [SerializeField] private TextMeshProUGUI _reviewSelectedServiceText;
        [SerializeField] private Button _reviewRequestButton;

        [Header("Leaderboards:")]
        [SerializeField] private TextMeshProUGUI _leaderboardSelectedServiceText;
        [SerializeField] private Button _leaderboardScore1Button;
        [SerializeField] private Button _leaderboardScore2Button;
        [SerializeField] private Button _leaderboardScore3Button;
        [SerializeField] private Button _leaderboardScore4Button;

        [Header("Materials:")]
        [SerializeField] private Material _originMaterial;
        [SerializeField] private Material _successMaterial;
        [SerializeField] private Material _errorMaterial;
        [SerializeField] private Material _pendingMaterial;


        private readonly Holder<IEnvironment> environment = new();
        private readonly Holder<IAds> ads = new();
        private readonly Holder<ISave> save = new();
        private readonly Holder<IPurchase> purchase = new();
        private readonly Holder<IAnalytics> analytics = new();
        private readonly Holder<IReview> review = new();
        private readonly Holder<ILeaderboards> leaderboards = new();



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

            _leaderboardScore1Button.onClick.AddListener(() => leaderboards.Item.SetScore(1));
            _leaderboardScore2Button.onClick.AddListener(() => leaderboards.Item.SetScore(3));
            _leaderboardScore3Button.onClick.AddListener(() => leaderboards.Item.SetScore(15));
            _leaderboardScore4Button.onClick.AddListener(() => leaderboards.Item.SetScore(99));

            Display();
        }

        private void Display()
        {
            _versionText.text = $"DevNote  {Info.VERSION}";

            _environmentSelectedServiceText.text = environment.Item.GetType().Name.Replace("EnvironmentService", string.Empty);
            _adsSelectedServiceText.text = ads.Item.GetType().Name.Replace("AdsService", string.Empty);
            _savesSelectedServiceText.text = save.Item.GetType().Name.Replace("SaveService", string.Empty);
            _purchasesSelectedServiceText.text = purchase.Item.GetType().Name.Replace("PurchaseService", string.Empty);
            _analyticsSelectedServiceText.text = analytics.Item.GetType().Name.Replace("AnalyticsService", string.Empty);
            _reviewSelectedServiceText.text = review.Item.GetType().Name.Replace("ReviewService", string.Empty);
            _leaderboardSelectedServiceText.text = leaderboards.Item.GetType().Name.Replace("LeaderboardsService", string.Empty);

            var usedSaveTime = ISave.UsedSaveTime;
            _usedSaveTimeText.text = usedSaveTime.ToString();

            string testValue = IEnvironment.IsTest ? "Active" : "Disabled";
            _environmentTestEnabledText.text = _environmentTestEnabledText.text.Replace("<test>", testValue);

            string environmentTypeValue = IEnvironment.EnvironmentKey.ToString();
            _environmentSelectedTypeText.text = _environmentSelectedTypeText.text.Replace("<type>", environmentTypeValue);

            string languageValue = environment.Item.DeviceLanguage.ToString();
            _environmentLanguageText.text = _environmentLanguageText.text.Replace("<language>", languageValue);

            string controlValue = environment.Item.DeviceType.ToString();
            _environmentDeviceTypeText.text = _environmentDeviceTypeText.text.Replace("<device>", controlValue);

            string priceValue = purchase.Item.GetPriceString(IProductKey.NoAds);
            _purchasesProductPriceText.text = _purchasesProductPriceText.text.Replace("<price>", priceValue);
            _purchasesProductKeyText.text = _purchasesProductKeyText.text.Replace("<key>", IProductKey.NoAds.ToString());

            DisplayPurchaseButton();

        }

        private void DisplayPurchaseButton()
        {
            bool noAdsPurchased = IGameState.NoAdsPurchased.Value;
            _purchasesPurchaseButton.image.material = noAdsPurchased ? _successMaterial : _originMaterial;
            _purchasesPurchaseButton.interactable = !noAdsPurchased;
            _purchaseButtonText.text = noAdsPurchased ? "Куплено" : "Купить";
        }



        private void OnDisableBannerButtonClick() => ads.Item.SetBanner(false);
        private void OnEnableBannerButtonClick() => ads.Item.SetBanner(true);

        private void OnReviewButtonClick() => review.Item.Rate();

        private void OnSendTestEventButtonClick() => analytics.Item.SendEvent("test_event", new Dictionary<string, object>()
        {
            { "random_int" , Random.Range(0, 3) },
            { "device_type" , environment.Item.DeviceType.ToString() },
        });

        private void OnPurchaseButtonClick()
        {
            _purchasesPurchaseButton.image.material = _pendingMaterial;

            purchase.Item.Purchase(IProductKey.NoAds,
                onSuccess: () => DisplayPurchaseButton(),
                onError: () => _purchasesPurchaseButton.image.material = _errorMaterial);
        }

        private void OnSaveCloudButtonClick()
        {
            _savesSaveCloudButton.image.material = _pendingMaterial;

            save.Item.SaveCloud(
                onSuccess: () => _savesSaveCloudButton.image.material = _successMaterial,
                onError: () => _savesSaveCloudButton.image.material = _errorMaterial);
        }

        private void OnSaveLocalButtonClick()
        {
            _savesSaveLocalButton.image.material = _pendingMaterial;

            save.Item.SaveLocal(
                onSuccess: () => _savesSaveLocalButton.image.material = _successMaterial,
                onError: () => _savesSaveLocalButton.image.material = _errorMaterial);
        }



        private void OnShowInterstitialButtonClick()
        {
            _adsShowInterstitialButton.image.material = _pendingMaterial;

            ads.Item.ShowInterstitial(callback: (status) =>
            {
                bool success = status == AdShowStatus.Success;
                _adsShowInterstitialButton.image.material = success ? _successMaterial : _errorMaterial;
            });
                
        }

        private void OnShowRewardedButtonClick()
        {
            _adsShowRewardedButton.image.material = _pendingMaterial;

            ads.Item.ShowRewarded(callback: (status) =>
            {
                bool success = status == AdShowStatus.Success;
                _adsShowRewardedButton.image.material = success ? _successMaterial : _errorMaterial;
            });

        }


        



    }
}

