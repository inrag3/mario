using UnityEngine;
using UnityEngine.UI;

namespace Bank
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private Text _text;

        public void Change(string currency)
        {
            _text.text = currency;
        }
    }
}