using System.Threading.Tasks;
using ApiCoreDapperCrudPagination.Dtos.Responses.Todos;
using ApiCoreDapperCrudPagination.Entities;
using ApiCoreDapperCrudPagination.Enums;
using ApiCoreDapperCrudPagination.Infrastructure.Services;
using ApiCoreDapperCrudPagination.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCoreDapperCrudPagination.Controllers
{
    [Route("api/[controller]")]
    public class TodosController : Controller
    {
        private readonly ITodoService _todosService;

        public TodosController(ITodoService todosService)
        {
            _todosService = todosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = await _todosService.FetchMany(page, pageSize);
            return StatusCodeAndDtoWrapper.BuildSuccess(TodoListResponse.Build(result.Item2, Request.Path, page,
                pageSize, result.Item1));
        }

        [HttpGet]
        [Route("pending")]
        public async Task<IActionResult> GetPending([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = await _todosService.FetchMany(page, pageSize, TodoShow.Pending);
            return StatusCodeAndDtoWrapper.BuildSuccess(TodoListResponse.Build(result.Item2, Request.Path, page,
                pageSize, result.Item1));
        }

        [HttpGet]
        [Route("completed")]
        public async Task<IActionResult> GetCompleted([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = await _todosService.FetchMany(page, pageSize, TodoShow.Completed);
            return StatusCodeAndDtoWrapper.BuildSuccess(TodoListResponse.Build(result.Item2, Request.Path, page,
                pageSize, result.Item1));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTodoDetails(int id)
        {
            var todo = await _todosService.GetById(id);
            if (todo == null)
                return StatusCodeAndDtoWrapper.BuildNotFound("Todo was not found");
            return StatusCodeAndDtoWrapper.BuildSuccess(TodoDetailsDto.Build(todo));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] Todo todo)
        {
            await _todosService.CreateTodo(todo);
            return StatusCodeAndDtoWrapper.BuildSuccess(TodoDetailsDto.Build(todo), "Todo Created Successfully");
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] Todo todo)
        {
            var todoFromDb = await _todosService.GetProxyById(id);
            if (todoFromDb == null)
                return StatusCodeAndDtoWrapper.BuildNotFound("Specified Todo was not found");

            return StatusCodeAndDtoWrapper.BuildSuccess(
                TodoDetailsDto.Build(await _todosService.Update(todoFromDb, todo)),
                "Todo Updated Successfully");
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todoFromDb = await _todosService.GetProxyById(id);
            if (todoFromDb == null)
                return StatusCodeAndDtoWrapper.BuildNotFound("Specified Todo was not found");
            await _todosService.Delete(id);
            return StatusCodeAndDtoWrapper.BuildSuccess("Todo Deleted Successfully");
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await _todosService.DeleteAll();
            return StatusCodeAndDtoWrapper.BuildSuccess("Todos Deleted Successfully");
        }
    }
}