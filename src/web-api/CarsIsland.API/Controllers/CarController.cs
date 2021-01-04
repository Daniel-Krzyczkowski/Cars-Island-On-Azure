using CarsIsland.Core.Entities;
using CarsIsland.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsIsland.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly IDataRepository<Car> _carRepository;

        public CarController(IDataRepository<Car> carRepository)
        {
            _carRepository = carRepository
                             ?? throw new ArgumentNullException(nameof(carRepository));
        }

        /// <summary>
        /// Gets list with available cars for rent
        /// </summary>
        /// <returns>
        /// List with available cars for rent
        /// </returns> 
        /// <response code="200">List with cars</response>
        /// <response code="401">Access denied</response>
        /// <response code="404">Cars list not found</response>
        /// <response code="500">Oops! something went wrong</response>
        [ProducesResponseType(typeof(IReadOnlyList<Car>), 200)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCarsAsync()
        {
            var allCars = await _carRepository.GetAllAsync();
            return Ok(allCars);
        }

        /// <summary>
        /// Gets details about specific car
        /// </summary>
        /// <returns>
        /// Details about specific car
        /// </returns> 
        /// <response code="200">Details about specific car</response>
        /// <response code="401">Access denied</response>
        /// <response code="404">Car not found</response>
        /// <response code="500">Oops! something went wrong</response>
        [ProducesResponseType(typeof(IReadOnlyList<Car>), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarDetailsAsync(string id)
        {
            var carDetails = await _carRepository.GetAsync(id);
            return Ok(carDetails);
        }
    }
}
