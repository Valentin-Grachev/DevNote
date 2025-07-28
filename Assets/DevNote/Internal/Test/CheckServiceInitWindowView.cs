using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckServiceInitWindowView : MonoBehaviour
{
    [SerializeField] private Color _successColor;
    [Space(10)]
    [SerializeField] private Image _environmentImage;
    [SerializeField] private Image _saveImage;
    [SerializeField] private Image _adsImage;
    [SerializeField] private Image _purchaseImage;
    [SerializeField] private Image _analyticsImage;
    [SerializeField] private Image _reviewImage;
    [SerializeField] private Image _soundImage;
    [SerializeField] private Image _localizationImage;
    [SerializeField] private Image _googleTablesImage;
    [SerializeField] private TextMeshProUGUI _versionText;

    private readonly Holder<IEnvironment> environment = new();
    private readonly Holder<ISave> save = new();
    private readonly Holder<IAds> ads = new();
    private readonly Holder<IPurchase> purchase = new();
    private readonly Holder<IAnalytics> analytics = new();
    private readonly Holder<IReview> review = new();


    private void Start()
    {
        if (!IEnvironment.IsTest) gameObject.SetActive(false);
        _versionText.text = Const.VERSION;
    }


    private void Update()
    {
        if (environment.Item.Initialized) _environmentImage.color = _successColor;
        if (save.Item.Initialized) _saveImage.color = _successColor;
        if (ads.Item.Initialized) _adsImage.color = _successColor;
        if (purchase.Item.Initialized) _purchaseImage.color = _successColor;
        if (analytics.Item.Initialized) _analyticsImage.color = _successColor;
        if (review.Item.Initialized) _reviewImage.color = _successColor;

        if (Sound.Initialized) _soundImage.color = _successColor;
        if (Localization.Initialized) _localizationImage.color = _successColor;
        if (GoogleTables.Initialized) _googleTablesImage.color = _successColor;

    }

}
