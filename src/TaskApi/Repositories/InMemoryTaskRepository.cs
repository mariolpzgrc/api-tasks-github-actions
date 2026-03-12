using TaskApi.Models;

namespace TaskApi.Repositories;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public InMemoryTaskRepository()
    {
        // Inicializar con 5 registros de prueba
        // _tasks.AddRange(new[]
        // {
        //     new TaskItem { Id = _nextId++, Title = "Tarea 1", Description = "Descripción de la tarea 1", IsCompleted = false },
        //     new TaskItem { Id = _nextId++, Title = "Tarea 2", Description = "Descripción de la tarea 2", IsCompleted = true },
        //     new TaskItem { Id = _nextId++, Title = "Tarea 3", Description = "Descripción de la tarea 3", IsCompleted = false },
        //     new TaskItem { Id = _nextId++, Title = "Tarea 4", Description = "Descripción de la tarea 4", IsCompleted = false },
        //     new TaskItem { Id = _nextId++, Title = "Tarea 5", Description = "Descripción de la tarea 5", IsCompleted = true }
        // });
    }

    public IEnumerable<TaskItem> GetAll() => _tasks.AsReadOnly();

    public TaskItem? GetById(int id) =>
        _tasks.FirstOrDefault(t => t.Id == id);

    public TaskItem Add(TaskItem task)
    {
        task.Id = _nextId++;
        _tasks.Add(task);
        return task;
    }

    public TaskItem? Update(int id, TaskItem updated)
    {
        var existing = GetById(id);
        if (existing is null) return null;

        existing.Title       = updated.Title;
        existing.Description = updated.Description;
        existing.IsCompleted = updated.IsCompleted;
        return existing;
    }

    public bool Delete(int id)
    {
        var task = GetById(id);
        if (task is null) return false;
        _tasks.Remove(task);
        return true;
    }
}