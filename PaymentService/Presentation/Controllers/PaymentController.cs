using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePayment(CreatePaymentDto dto)
    {
        var result = await _paymentService.CreateAsync(dto);

        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }
        return CreatedAtAction(nameof(GetByIdAsync), new { id = result.PaymentDto.Id }, result.PaymentDto);
    }
    
    [HttpGet("{id}")]
    [ActionName(nameof(GetByIdAsync))]
    public async Task<ActionResult<PaymentDto>> GetByIdAsync(int id)
    {
        var paymentDto = await _paymentService.GetByIdAsync(id);
        if (paymentDto == null)
        {
            return NotFound($"Payment {id} not found");
        }
        
        return Ok(paymentDto);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<PaymentDto>>> GetAllAsync()
    {
        var payments = await _paymentService.GetAllAsync();
        
        if (!payments.Any())
        {
            return NotFound("No payments found");
        }
        
        return Ok(payments);
    }
}