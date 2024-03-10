﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Studenda.Server.Middleware.Security.Requirement;
using Studenda.Server.Model.Common;
using Studenda.Server.Service;

namespace Studenda.Server.Controller;

/// <summary>
///     Контроллер для работы с объектами типа <see cref="Account" />.
/// </summary>
/// <param name="accountService">Сервис моделей.</param>
[Route("api/account")]
[ApiController]
public class AccountController(AccountService accountService) : ControllerBase
{
    /// <summary>
    ///     Сервис моделей.
    /// </summary>
    private AccountService AccountService { get; } = accountService;

    /// <summary>
    ///     Получить список аккаунтов.
    ///     Если идентификаторы не указаны, возвращается список со всеми аккаунтами.
    ///     Иначе возвращается список с указанными аккаунтами, либо пустой список.
    /// </summary>
    /// <param name="ids">Список идентификаторов.</param>
    /// <returns>Результат операции со списком аккаунтов.</returns>
    [HttpGet]
    public async Task<ActionResult<List<Account>>> Get([FromQuery] List<int> ids)
    {
        return await AccountService.Get(AccountService.DataContext.Accounts, ids);
    }

    /// <summary>
    ///     Получить список аккаунтов по идентификаторам групп.
    /// </summary>
    /// <param name="groupIds">Идентификаторы групп.</param>
    /// <returns>Результат операции со списком аккаунтов.</returns>
    public async Task<ActionResult<List<Account>>> GetByGroup([FromQuery] List<int> groupIds)
    {
        return await AccountService.GetByGroup(groupIds);
    }

    /// <summary>
    ///     Сохранить аккаунты.
    /// </summary>
    /// <param name="entities">Список аккаунтов.</param>
    /// <returns>Результат операции.</returns>
    [Authorize(Policy = AdminRoleAuthorizationRequirement.AuthorizationPolicyCode)]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] List<Account> entities)
    {
        var status = await AccountService.Set(AccountService.DataContext.Accounts, entities);

        if (!status)
        {
            return BadRequest("No accounts were saved!");
        }

        return Ok();
    }

    /// <summary>
    ///     Удалить аккаунты.
    /// </summary>
    /// <param name="ids">Список идентификаторов.</param>
    /// <returns>Результат операции.</returns>
    [Authorize(Policy = AdminRoleAuthorizationRequirement.AuthorizationPolicyCode)]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] List<int> ids)
    {
        var status = await AccountService.Remove(AccountService.DataContext.Accounts, ids);

        if (!status)
        {
            return BadRequest("No accounts were deleted!");
        }

        return Ok();
    }
}