using Microsoft.AspNetCore.Mvc;

using RapidPay.DTOs;
using RapidPay.Entities;
using RapidPay.Repositories;
using RapidPay.Services;
using RapidPay.Utilities;

namespace RapidPay.Controllers
{
    [ApiController]
    [Route("api/cards")]
    public class CardController : ControllerBase
    {
        private readonly CardRepository _cardRepository;
        private readonly UFEService _ufeService;

        public CardController(CardRepository cardRepository,
                                                        UFEService ufeService)
        {
            _cardRepository = cardRepository;
            _ufeService = ufeService;
        }

        [HttpPost("createCard")]
        public async Task<ActionResult> CreateCard()
        {
            var cardNumber = Utils.GenerateCardNumber();
            var card = new Card { Number = cardNumber };
            await _cardRepository.Add(card);
            return Ok(card);
        }

        [HttpPost("pay")]
        public async Task<ActionResult> Pay(PaymentDTO paymentDTO)
        {
            var card = await _cardRepository.Get(paymentDTO.CardNumber);
            if (card == null) { return NotFound("Card not found."); }

            var amount = paymentDTO.Amount;
            var fee = await _ufeService.CalculateFee();

            var total = amount + fee;
            if (card.Balance >= total)
            {
                card.Balance -= total;
                await _cardRepository.Update(card);
                return Ok();
            }
            return BadRequest("Insufficient balance");
        }

        [HttpGet("getBalance")]
        public async Task<ActionResult> GetBalance(string cardNumber)
        {
            var card = await _cardRepository.Get(cardNumber);
            if (card == null) { return NotFound(); }

            return Ok(card.Balance);
        }
    }   
}
