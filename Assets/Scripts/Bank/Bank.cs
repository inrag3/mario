using System;

namespace Bank
{
    public class Bank
    {
        public int Currency { get; private set; }

        public event Action<int> Added;

        public void Add(int currency)
        {
            Currency += currency;
            Added?.Invoke(Currency);
        }
    }
}