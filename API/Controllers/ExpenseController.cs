using API.Controllers.Common;
using Application.Dtos.Expense;
using Application.Features.Expense.Requests.Commands;
using Application.Features.Expense.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetExpenseListWithParam([FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var result = await _mediator.Send(new GetListOfAllExpenseWithParamsRequest
            {
                SortBy = sort_field,
                SortDirection = sort_order,
                Page = page,
                PerPage = per_page
            });
            return Ok(new
            {
                data = result.Data,
                meta = new
                {
                    total = result.Total,
                    current_page = result.CurrentPage,
                    per_page = result.PerPage,
                    last_page = result.LastPage,
                    from = result.From,
                    to = result.To
                }
            });
        }

        [HttpGet("get-all-list")]
        public async Task<IActionResult> GetAllExpenseList()
        {
            var buidings = await _mediator.Send(new GetListOfExpenseRequest());
            return Ok(buidings);
        }

        [HttpPost("add-expense")]
        public async Task<IActionResult> AddExpense(AddExpenseDto addExpenseDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new AddExpenseCommand { AddExpenseDto = addExpenseDto });
                return Ok(new { message = result.Message });
            }
            return BadRequest();
        }

        [HttpPost("update-expense")]
        public async Task<IActionResult> UpdateExpense(UpdateExpenseDto updateExpenseDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new UpdateExpenseCommand { UpdateExpenseDto = updateExpenseDto });
                return Ok(new { message = result.Message });
            }
            return BadRequest();
        }
    }
}