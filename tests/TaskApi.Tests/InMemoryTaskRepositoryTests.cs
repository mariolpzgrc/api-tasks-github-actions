using TaskApi.Repositories;
using TaskApi.Models;
using FluentAssertions;

namespace TaskApi.Tests.Repositories;

public class InMemoryTaskRepositoryTests
{
    private readonly InMemoryTaskRepository _repo;
    public InMemoryTaskRepositoryTests()
    {
        _repo = new();
    }

    [Fact]
    public void Add_TareaValida_AsignaIdYRetornaTarea()
    {
        //Arrange        
        TaskItem tarea = new()
        {
            Title = "Comprar Guitarra",
            Description = "Comprar Guitarra para ser Feliz"
        };
        //Act
        var resultado = _repo.Add(tarea);
        //Arrange
        resultado.Id.Should().BeGreaterThan(0);
        resultado.Title.Should().Be("Comprar Guitarra");
    }

    [Fact]
    public void Add_DosTareasAsignaIdsSecuenciales()
    {
        TaskItem tarea1 = new()
        {
            Title = "Comprar un sbos",
            Description = "Para jugar Halito"
        };

        TaskItem tarea2 = new()
        {
            Title = "Armar una PCMR",
            Description = "Quiero ser feliz"
        };

        var r1 = _repo.Add(tarea1);
        var r2 = _repo.Add(tarea2);

        r2.Id.Should().Be(r1.Id + 1);
    }

    [Fact]
    public void GetAll()
    {
        var resultado = _repo.GetAll();
        resultado.Should().BeEmpty();
    }

    [Fact]
    public void GetAll_ConDosTareas_RetornarDosTareas()
    {
        TaskItem tarea1 = new()
        {
            Title = "Comprar un sbos",
            Description = "Para jugar Halito"
        };

        TaskItem tarea2 = new()
        {
            Title = "Armar una PCMR",
            Description = "Quiero ser feliz"
        };

        var r1 = _repo.Add(tarea1);
        var r2 = _repo.Add(tarea2);

        var resultado = _repo.GetAll();

        resultado.Should().HaveCount(2);
    }

    [Fact]
    public void GetByID_TareaExiste_RetornarTarea()
    {
        TaskItem tarea = new()
        {
            Title = "Comprar Guitarra",
            Description = "Comprar Guitarra para ser Feliz"
        };
        //Act
        var tareaAgragada = _repo.Add(tarea);
        var resultado = _repo.GetById(tareaAgragada.Id);
        resultado.Should().NotBeNull();
        resultado.Title.Should().Be("Comprar Guitarra");
    }

    [Fact]
    public void GetById_IdNoExiste_ReturnaNull()
    {
        var resultado = _repo.GetById(666);
        resultado.Should().BeNull();
    }

    [Fact]
    public void Update_TareaExiste_Actualizar()
    {
        var tareaOriginal = _repo.Add(new TaskItem { Title = "Tarea 1", Description = "Descripción de la tarea 1" });
        var cambioTarea = new TaskItem { Title = "Actualizacion", Description = "Tarea 1 actualizada" };

        var resultado = _repo.Update(tareaOriginal.Id, cambioTarea);
        resultado.Should().NotBeNull();
        resultado.Title.Should().Be("Actualizacion");
    }

    [Fact]
    public void UpdateNoExistente()
    {
        var resultado = _repo.Update(100, new TaskItem());
        resultado.Should().BeNull();
    }

    [Fact]
    public void Delete_TareaExiste_RetornaTrue()
    {
        var tareaAgrgada = _repo.Add(new TaskItem { Title = "Tarea a eliminar" });
        var resultado = _repo.Delete(tareaAgrgada.Id);
        resultado.Should().BeTrue();
        _repo.GetById(tareaAgrgada.Id).Should().BeNull();
    }

    [Fact]
    public void Delete_TareaNoExiste_RetornaFalse()
    {
        var resultado = _repo.Delete(10);
        resultado.Should().BeFalse();
    }
}