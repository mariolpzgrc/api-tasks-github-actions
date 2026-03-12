using TaskApi.Models;
using TaskApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace TaskApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repo;

    public TasksController(ITaskRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public IActionResult GetAll() =>
        Ok(_repo.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var task = _repo.GetById(id);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpPost]
    public IActionResult Create(TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
            return BadRequest("El título es obligatorio.");

        var created = _repo.Add(task);
        return CreatedAtAction(nameof(GetById),
                               new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, TaskItem task)
    {
        var updated = _repo.Update(id, task);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _repo.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}