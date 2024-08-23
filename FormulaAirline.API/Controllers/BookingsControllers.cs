using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormulaAirline.API.Models;
using FormulaAirline.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormulaAirline.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingsControllers: ControllerBase
    {
        private readonly ILogger<BookingsControllers> _logger;
        private readonly IMessageProducer _messageProducer;
        public static readonly List<Booking> _bookings = new ();

        public BookingsControllers(ILogger<BookingsControllers> logger, IMessageProducer messageProducer)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public IActionResult CreateBooking (Booking newBooking){
            if(ModelState.IsValid == false)
                return BadRequest(ModelState);
            _bookings.Add(newBooking);
            _messageProducer.SendingMessage<Booking>(newBooking);
            return Ok();
        }

    }
}