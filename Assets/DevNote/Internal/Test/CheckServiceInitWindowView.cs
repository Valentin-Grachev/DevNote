using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckServiceInitWindowView : MonoBehaviour
{
    [SerializeField] private Material _successMaterial;
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
        _versionText.text = $"DevNote  {Info.VERSION}";
    }


    private void Update()
    {
        if (environment.Item.Initialized) _environmentImage.material = _successMaterial;
        if (save.Item.Initialized) _saveImage.material = _successMaterial;
        if (ads.Item.Initialized) _adsImage.material = _successMaterial;
        if (purchase.Item.Initialized) _purchaseImage.material = _successMaterial;
        if (analytics.Item.Initialized) _analyticsImage.material = _successMaterial;
        if (review.Item.Initialized) _reviewImage.material = _successMaterial;

        if (Sound.Initialized) _soundImage.material = _successMaterial;
        if (Localization.Initialized) _localizationImage.material = _successMaterial;
        if (GoogleTables.Initialized) _googleTablesImage.material = _successMaterial;

    }

}
