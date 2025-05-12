using Models;
using Services;
using UnityEngine;
using UnityEngine.UI;

public class UiSlotItem : MonoBehaviour
{
    [SerializeField] private Image Slot;
    [SerializeField] private Image Shape;
    [SerializeField] private Image BackGround;
    [SerializeField] private Image Icon;

    public Color ActiveSlot;
    public Color InactiveSlot;

    private PropsServices _propsServices;

    public void Init(PropsServices propsServices)
    {
        _propsServices = propsServices;
    }

    public void UpdateView(DataFigure dataFigure)
    {
        Slot.color = InactiveSlot;
        Shape.gameObject.SetActive(true);
        Shape.sprite = _propsServices.GetShape(dataFigure);
        BackGround.sprite = _propsServices.GetShape(dataFigure);
        BackGround.color = _propsServices.GetColor(dataFigure);
        Icon.sprite = _propsServices.GetIcon(dataFigure);
    }

    public void Clean()
    {
        Slot.color = ActiveSlot;
        Shape.gameObject.SetActive(false);
    }
}