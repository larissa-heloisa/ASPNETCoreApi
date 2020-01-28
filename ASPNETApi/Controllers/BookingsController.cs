﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Services.Administrators;
using Domain.Interfaces.Services.Bookings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private IBookingService _service;
        private IAdministratorService _serviceadmin;

        public BookingsController(IBookingService service, IAdministratorService serviceadmin)
        {
            _service = service;
            _serviceadmin = serviceadmin;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromServices] IBookingService service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await service.GetAll());
            }
            catch(ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("{id}", Name ="GetById")]
        public async Task<ActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _service.Get(id));
            }
            catch(ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InputCreateBooking createBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            { var booking = new Booking
            {
                Id = Guid.NewGuid(),
                Name = createBooking.Name,
                Description = createBooking.Description,
                Price = createBooking.Price,
                Administrator = await _serviceadmin.Get(createBooking.AdminId),
                Visitor = null
                
            };
                var result = await _service.Post(booking);
                if(result != null)
                {
                    return Created(new Uri(Url.Link("GetById", new { id = result.Id })), result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _service.Put(booking);
                if(result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await _service.Delete(id));
            }
            catch(ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}