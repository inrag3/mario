using System;

namespace Bank
{
    public class CurrencyPresenter : IInitiazable, IDisposable
    {
        private Bank _model;
        private CurrencyView _view;

        public CurrencyPresenter(Bank model, CurrencyView view)
        {
            _view = view;
            _model = model;
        }
        public void Initialize()
        {
            _model.Added += OnAdded;
        }

        public void Dispose()
        {
            _model.Added -= OnAdded;
        }

        private void OnAdded(int currency) => _view.Change($"x {currency}");
    }
}