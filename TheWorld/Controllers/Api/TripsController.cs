﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<TripsController> _logger;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = _repository.GetTripByUserName(this.User.Identity.Name);

                var res = (Mapper.Map<IEnumerable<TripViewModel>>(results));
                return Ok(res);
            }
            catch (Exception ex)
            {
                //TODO Logging
                _logger.LogError($"Failed to get All Trips: {ex}");

                return BadRequest("Error ocurred");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(theTrip);

                newTrip.UserName = User.Identity.Name;

                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }
               
                
            }

            return BadRequest("Failed to save the trip");
        }
    }
}
