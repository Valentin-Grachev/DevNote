using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

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

    [Inject] private readonly ISave environment;
    [Inject] private readonly ISave save;
    [Inject] private readonly IAds ads;
    [Inject] private readonly IPurchase purchase;
    [Inject] private readonly IAnalytics analytics;
    [Inject] private readonly IReview review;


    private void Start()
    {
        if (!IEnvironment.IsTest) gameObject.SetActive(false);
        _versionText.text = Const.VERSION;
    }


    private void Update()
    {
        if (environment.Initialized) _environmentImage.color = _successColor;
        if (save.Initialized) _saveImage.color = _successColor;
        if (ads.Initialized) _adsImage.color = _successColor;
        if (purchase.Initialized) _purchaseImage.color = _successColor;
        if (analytics.Initialized) _analyticsImage.color = _successColor;
        if (review.Initialized) _reviewImage.color = _successColor;

        if (Sound.Initialized) _soundImage.color = _successColor;
        if (Localization.Initialized) _localizationImage.color = _successColor;
        if (GoogleTables.Initialized) _googleTablesImage.color = _successColor;

    }

}
