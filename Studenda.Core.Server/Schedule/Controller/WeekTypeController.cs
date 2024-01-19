using Microsoft.AspNetCore.Mvc;
using Studenda.Core.Model.Schedule.Management;
using Studenda.Core.Server.Schedule.Service;

namespace Studenda.Core.Server.Schedule.Controller;

/// <summary>
///     Контроллер для работы с объектами типа <see cref="WeekType" />.
/// </summary>
[Route("api/week-type")]
[ApiController]
public class WeekTypeController : ControllerBase
{
    /// <summary>
    ///     Конструктор.
    /// </summary>
    /// <param name="weekTypeService">Сервис типов недель.</param>
    public WeekTypeController(WeekTypeService weekTypeService)
    {
        WeekTypeService = weekTypeService;
    }

    /// <summary>
    ///     Сервис моделей.
    /// </summary>
    private WeekTypeService WeekTypeService { get; }

    /// <summary>
    ///     Получить текущий тип недели.
    /// </summary>
    /// <param name="year">Год.</param>
    /// <returns>Результат операции с типом недели или пустой результат.</returns>
    [HttpGet("current/{year:int}")]
    public ActionResult<WeekType?> GetCurrent([FromQuery] int year)
    {
        return WeekTypeService.GetCurrent(year);
    }

    /// <summary>
    ///     Получить список типов недель.
    ///     Если идентификаторы не указаны, возвращается список со всеми типами недель.
    ///     Иначе возвращается список с типами недель.
    /// </summary>
    /// <param name="ids">Список идентификаторов.</param>
    /// <returns>Результат операции со списком типов недель.</returns>
    [HttpGet]
    public ActionResult<List<WeekType>> Get([FromBody] List<int> ids)
    {
        return WeekTypeService.Get(WeekTypeService.DataContext.WeekTypes, ids);
    }

    /// <summary>
    ///     Сохранить типы недель.
    /// </summary>
    /// <param name="entities">Список типов недель.</param>
    /// <returns>Результат операции.</returns>
    [HttpPost]
    public IActionResult Post([FromBody] List<WeekType> entities)
    {
        var status = WeekTypeService.Set(entities);

        if (!status)
        {
            return BadRequest("No week types were saved!");
        }

        return Ok();
    }

    /// <summary>
    ///     Удалить типы недель.
    /// </summary>
    /// <param name="ids">Список идентификаторов.</param>
    /// <returns>Результат операции.</returns>
    [HttpDelete]
    public IActionResult Delete([FromBody] List<int> ids)
    {
        var status = WeekTypeService.Remove(ids);

        if (!status)
        {
            return BadRequest("No week types were deleted!");
        }

        return Ok();
    }

}