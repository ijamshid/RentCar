﻿using RentCar.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.DTOs
{
    public class CarCreateDto
    {
        public int BrandId { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string PlateNumber { get; set; }

        public FuelType FuelType { get; set; }

        public decimal DailyPrice { get; set; }

        public int? Odometer { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }
    }
}
