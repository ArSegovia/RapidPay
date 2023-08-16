using System;

using RapidPay.Entities;

namespace RapidPay.Services
{
    public class UFEService
    {
        private readonly object _thread = new object();
        private readonly Random _random = new Random();
        private decimal _lastFee = 1.0M;
        private DateTime _lastFeeUpdateTime = DateTime.MinValue;

        public async Task<decimal> CalculateFee()
        {
            await Task.Run(() =>
            {
                lock (_thread)
                {
                    var currentTime = DateTime.Now;
                    if (currentTime.Hour != _lastFeeUpdateTime.Hour)
                    {
                        var randomDecimal = Convert.ToDecimal(_random.NextDouble() * 2);
                        _lastFee *= randomDecimal;
                        _lastFeeUpdateTime = currentTime;
                    }
                }
            });
            return _lastFee;
        }
    }
}
