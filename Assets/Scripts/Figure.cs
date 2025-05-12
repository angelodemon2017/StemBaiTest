using Models;
using Services;
using Signals;
using UnityEngine;
using Zenject;

public class Figure : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _backGround;
    [SerializeField] private SpriteRenderer _iconFigure;

    private SignalBus _signalBus;
    public DataFigure _dataFigure;

    [Inject]
    public void Init(SignalBus signalBus, PropsServices propsServices, DataFigure dataFigure)
    {
        _signalBus = signalBus;
        _dataFigure = dataFigure;

        UpdateView(propsServices);
    }

    private void UpdateView(PropsServices propsServices)
    {
        _backGround.color = propsServices.GetColor(_dataFigure);
        _iconFigure.sprite = propsServices.GetIcon(_dataFigure);
    }

    private void OnMouseDown()
    {
        _signalBus.Fire(new ClickOnFigureSignal(_dataFigure));
        Destroy(gameObject);
    }
}