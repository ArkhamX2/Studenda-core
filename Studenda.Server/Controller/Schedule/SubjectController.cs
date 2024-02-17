using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Studenda.Server.Model.Schedule;
using Studenda.Server.Service.Schedule;

namespace Studenda.Server.Controller.Schedule;

/// <summary>
///     Контроллер для работы с объектами типа <see cref="Subject" />.
/// </summary>
/// <param name="subjectService">Сервис статичных занятий.</param>
[Route("api/schedule/subject")]
[ApiController]
public class SubjectController(SubjectService subjectService) : ControllerBase
{
    /// <summary>
    ///     Сервис статичных занятий.
    /// </summary>
    private SubjectService SubjectService { get; } = subjectService;

    /// <summary>
    ///     Получить список статичных занятий.
    ///     Если идентификаторы не указаны, возвращается список со всеми занятиями.
    ///     Иначе возвращается список с указанными занятиями, либо пустой список.
    /// </summary>
    /// <param name="ids">Список идентификаторов.</param>
    /// <returns>Результат операции со списком занятий.</returns>
    [HttpGet]
    public async Task<ActionResult<List<Subject>>> Get([FromQuery] List<int> ids)
    {
        return await SubjectService.Get(SubjectService.DataContext.Subjects, ids);
    }

    /// <summary>
    ///     Получить список статичных занятий по идентификатору группы.
    /// </summary>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <param name="weekTypeId">Идентификатор типа недели.</param>
    /// <param name="year">Учебный год.</param>
    /// <returns>Результат операции со списком статичных занятий.</returns>
    [HttpGet]
    [Route("group")]
    public async Task<ActionResult<List<Subject>>> GetByGroup([FromQuery] int groupId, [FromQuery] int weekTypeId, [FromQuery] int year)
    {
        return await SubjectService.GetSubjectByGroup(groupId, weekTypeId, year);
    }

    /// <summary>
    ///     Получить список статичных занятий по идентификатору пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="weekTypeId">Идентификатор типа недели.</param>
    /// <param name="year">Учебный год.</param>
    /// <returns>Результат операции со списком статичных занятий.</returns>
    [HttpGet]
    [Route("user")]
    public async Task<ActionResult<List<Subject>>> GetByUser([FromQuery] string userId, [FromQuery] int weekTypeId, [FromQuery] int year)
    {
        return await SubjectService.GetSubjectByUser(userId, weekTypeId, year);
    }

    /// <summary>
    ///     Сохранить статичные занятия.
    /// </summary>
    /// <param name="entities">Список статичных занятий.</param>
    /// <returns>Результат операции.</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] List<Subject> entities)
    {
        var status = await SubjectService.Set(SubjectService.DataContext.Subjects, entities);

        if (!status)
        {
            return BadRequest("No subjects were saved!");
        }

        return Ok();
    }

    /// <summary>
    ///     Удалить статичные занятия.
    /// </summary>
    /// <param name="ids">Список идентификаторов.</param>
    /// <returns>Результат операции.</returns>
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] List<int> ids)
    {
        var status = await SubjectService.Remove(SubjectService.DataContext.Subjects, ids);

        if (!status)
        {
            return BadRequest("No subjects were deleted!");
        }

        return Ok();
    }
}