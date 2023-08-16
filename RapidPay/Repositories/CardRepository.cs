using RapidPay.Entities;

namespace RapidPay.Repositories
{
    public class CardRepository
    {
        private readonly object _thread = new object();
        private readonly Dictionary<string, Card> _cards = new Dictionary<string, Card>();
        public async Task Add(Card card)
        {
            await Task.Run(() =>
            {
                lock (_thread) { _cards[card.Number] = card; }
            });
        }

        public async Task<Card> Get(string cardNumber)
            => await Task.Run(() =>
                    {
                        lock (_thread)
                        {
                            _cards.TryGetValue(cardNumber, out var card);
                            return card;
                        }
                    });

        public async Task Update(Card card)
        {
            await Task.Run(() =>
            {
                lock (_thread) { _cards[card.Number] = card; }
            });
        }
    }
}
