using Microsoft.AspNetCore.Mvc;
using Sprint03.adapter.input.dto;
using Sprint03.domain.model;
using Sprint03.infra.exception;
using Sprint03.infra.service.dto;

namespace Sprint03.app.controller
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerAdapter _customerAdapter;
        private readonly IAiService _aiService;

        public CustomerController(ICustomerAdapter customerAdapter, IAiService aiService)
        {
            _customerAdapter = customerAdapter;
            _aiService = aiService;
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> FindById(string id)
        {
            try
            {
                var customer = _customerAdapter.FindById(id);
                return Ok(customer);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidIdFormatException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<Customer> Create([FromBody] Customer customer)
        {
            try
            {
                _customerAdapter.Create(customer);
                return CreatedAtAction(nameof(FindById), new { id = customer.Id }, customer);
            }
            catch (InvalidException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Customer> Update(string id, [FromBody] Customer customer)
        {
            try
            {
                var updatedCustomer = _customerAdapter.Update(id, customer);
                return Ok(updatedCustomer);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidIdFormatException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                _customerAdapter.Delete(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidIdFormatException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        
        [HttpGet("validate-cep/{cep}")]
        public async Task<IActionResult> ValidateCep(string cep)
        {
            try
            {
                var isValid = await _customerAdapter.ValidateCepAsync(cep);
                if (isValid)
                {
                    return Ok(new { message = "CEP válido." });
                }
                return BadRequest(new { message = "CEP inválido." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        
        [HttpGet("{id}/recommendations")]
        public ActionResult<List<string>> GetProductRecommendations(string id)
        {
            var customer = _customerAdapter.FindById(id);
            var recommendations = _aiService.RecommendProducts(customer);
            return Ok(recommendations);
        }

        [HttpPost("analyze-sentiment")]
        public ActionResult<string> AnalyzeSentiment([FromBody] SentimentRequest request)
        {
            if (string.IsNullOrEmpty(request.Text))
            {
                return BadRequest("The text field is required.");
            }
    
            var sentiment = _aiService.AnalyzeSentiment(request.Text);
            return Ok(new { Sentiment = sentiment });
        }

    }
}
