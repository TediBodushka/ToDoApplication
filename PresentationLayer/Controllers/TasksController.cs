using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskItemContext _taskContext;

        public TasksController(ToDoListDbContext dbContext)
        {
            _taskContext = new TaskItemContext(dbContext);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_taskContext.ReadAll(useNavigationalProperties: true));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_taskContext.Read(id, useNavigationalProperties: true));
        }

        [HttpPost]
        public IActionResult Create(TaskItem task)
        {
            _taskContext.Create(task);
            return Ok(task);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TaskItem task)
        {
            task.Id = id;
            _taskContext.Update(task);
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _taskContext.Delete(id);
            return Ok();
        }
    }
}
