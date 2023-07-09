using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class MoneyBurst : MonoBehaviour
{
    [SerializeField] private RectTransform[] moneyImagesArray;

    [SerializeField] private RectTransform target;

    [SerializeField] private float firstBurstDuration;
    [SerializeField] private float firstBurstAmount;
    [SerializeField] private float deliverToTargetDuration;

    public void Burst()
    {
        var _sequence = DOTween.Sequence();

        foreach (var _image in moneyImagesArray)
        {
            _image.gameObject.SetActive(true);
            _image.ResetLocalTransform();
            var _burstPosition = Random.insideUnitCircle * firstBurstAmount;
            _sequence.Join(_image.DOAnchorPos(_burstPosition, firstBurstDuration).SetEase(Ease.OutQuint));
        }

        _sequence.OnComplete(() =>
        {
            foreach (var _image in moneyImagesArray)
            {
                var _img = _image;
                _image.DOMove(target.position, deliverToTargetDuration).SetEase(Ease.InOutQuart).OnComplete(() =>
                {
                    _img.gameObject.SetActive(false);
                });
            }
        });
    }

    public void Burst(Vector2 position)
    {
        GetComponent<RectTransform>().anchoredPosition = position;

        var _sequence = DOTween.Sequence();

        foreach (var _image in moneyImagesArray)
        {
            _image.gameObject.SetActive(true);
            _image.ResetLocalTransform();
            var _burstPosition = Random.insideUnitCircle * firstBurstAmount;
            _sequence.Join(_image.DOAnchorPos(_burstPosition, firstBurstDuration).SetEase(Ease.OutQuint));
        }

        _sequence.OnComplete(() =>
        {
            foreach (var _image in moneyImagesArray)
            {
                var _img = _image;
                _image.DOMove(target.position, deliverToTargetDuration).SetEase(Ease.InOutQuart).OnComplete(() =>
                {
                    _img.gameObject.SetActive(false);
                });
            }
        });
    }
}