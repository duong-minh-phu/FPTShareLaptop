﻿using DataAccess.DonationFormDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Net;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonateFormController : ControllerBase
    {
        private readonly IDonateFormService _donationFormService;

        public DonateFormController(IDonateFormService donationFormService)
        {
            _donationFormService = donationFormService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllDonations()
        {
            var result = await _donationFormService.GetAllDonationsAsync();

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get all donation forms successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDonationById(int id)
        {
            var result = await _donationFormService.GetDonationByIdAsync(id);
            if (result == null)
            {
                return NotFound(new ResultModel { IsSuccess = false, Code = (int)HttpStatusCode.NotFound, Message = "Donation form not found" });
            }

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get donation form successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateDonation([FromForm] CreateDonateFormReqModel request)
        {
            var result = await _donationFormService.CreateDonationAsync(request);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Donation form created successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateDonation(int id, [FromBody] UpdateDonateFormReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _donationFormService.UpdateDonationAsync(id, request);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Donation form updated successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> SoftDeleteDonation(int id)
        {
            await _donationFormService.DeleteDonationAsync(id);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Donation form deleted successfully",
            };

            return StatusCode(response.Code, response);
        }
    }
}