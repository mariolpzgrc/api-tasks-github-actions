using TaskApi.Controllers;
using TaskApi.Models;
using TaskApi.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace TaskApi.Tests.Controllers;

public class TaskControllerTests
{
    private readonly TasksController _ctrl;
    private readonly Mock<ITaskRepository> _mockRepo;

    public TaskControllerTests()
    {
        _mockRepo = new Mock<ITaskRepository>();
        _ctrl = new TasksController(_mockRepo.Object);
    }

    [Fact]
    public void GetALl_HayTareas_RetornaOKConListaDeTareas()
    {
        _mockRepo.Setup(r => r.GetAll()).Returns(
            [
                new (){Title ="Title 1"},
                new (){Title ="Title 2"}
            ]
        );

        _ctrl.GetAll().Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<TaskItem>>();
    }

    //GetByID
    [Fact]
    public void GetById_ReturnsOkResult_WithTaskItem()
    {
        _mockRepo.Setup(repo => repo.GetById(1)).Returns(new TaskItem { Id = 1, Title = "Tarea 1", Description = "Descripción de la tarea 1" });
        _ctrl.GetById(1)
            .Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<TaskItem>()
            .Which.Title.Should().Be("Tarea 1");
    }

    [Fact]
    public void GetById_TaskNotFound_RetornaNotFoundResult()
    {
        _mockRepo.Setup(repo => repo.GetById(666)).Returns((TaskItem?)null);
        _ctrl.GetById(666)  
            .Should().BeOfType<NotFoundResult>();
    }

    // Create
    [Fact]
    public void Create_ValidTask_ReturnsCreatedAtAction()
    {
        var newTask = new TaskItem { Title = "Nueva Tarea", Description = "Descripción nueva" };
        var createdTask = new TaskItem { Id = 1, Title = "Nueva Tarea", Description = "Descripción nueva" };
        
        _mockRepo.Setup(repo => repo.Add(It.IsAny<TaskItem>())).Returns(createdTask);
        
        _ctrl.Create(newTask)
            .Should().BeOfType<CreatedAtActionResult>()
            .Which.ActionName.Should().Be(nameof(TasksController.GetById));
    }

    [Fact]
    public void Create_EmptyTitle_ReturnsBadRequest()
    {
        var invalidTask = new TaskItem { Title = "", Description = "Descripción sin título" };
        
        _ctrl.Create(invalidTask)
            .Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void Create_NullTitle_ReturnsBadRequest()
    {
        var invalidTask = new TaskItem { Title = null, Description = "Descripción sin título" };
        
        _ctrl.Create(invalidTask)
            .Should().BeOfType<BadRequestObjectResult>();
    }

    // Update
    [Fact]
    public void Update_ValidTask_ReturnsOkWithUpdatedTask()
    {
        var updatedTask = new TaskItem { Id = 1, Title = "Actualizada", Description = "Descripción actualizada" };
        _mockRepo.Setup(repo => repo.Update(1, It.IsAny<TaskItem>())).Returns(updatedTask);
        
        _ctrl.Update(1, updatedTask)
            .Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<TaskItem>()
            .Which.Title.Should().Be("Actualizada");
    }

    [Fact]
    public void Update_TaskNotFound_ReturnsNotFound()
    {
        var taskToUpdate = new TaskItem { Title = "Actualizada", Description = "Descripción" };
        _mockRepo.Setup(repo => repo.Update(999, It.IsAny<TaskItem>())).Returns((TaskItem?)null);
        
        _ctrl.Update(999, taskToUpdate)
            .Should().BeOfType<NotFoundResult>();
    }

    // Delete
    [Fact]
    public void Delete_TaskExists_ReturnsNoContent()
    {
        _mockRepo.Setup(repo => repo.Delete(1)).Returns(true);
        
        _ctrl.Delete(1)
            .Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void Delete_TaskNotFound_ReturnsNotFound()
    {
        _mockRepo.Setup(repo => repo.Delete(666)).Returns(false);
        
        _ctrl.Delete(666)
            .Should().BeOfType<NotFoundResult>();
    }

    // GetAll
    [Fact]
    public void GetAll_EmptyList_ReturnsEmptyList()
    {
        _mockRepo.Setup(r => r.GetAll()).Returns([]);
        
        _ctrl.GetAll().Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<TaskItem>>()
            .Which.Should().BeEmpty();
    }
}