using Microsoft.EntityFrameworkCore;
using Studenda.Server.Data;
using Studenda.Server.Model.Security;

namespace Studenda.Server.Service.Security;

/// <summary>
///     Сервис для работы с <see cref="Account" />.
/// </summary>
/// <param name="dataContext">Контекст данных.</param>
public class AccountService(DataContext dataContext) : DataEntityService(dataContext)
{
    /// <summary>
    ///     Получить список аккаунтов по идентификаторам ролей.
    /// </summary>
    /// <param name="roleIds">Идентификаторы ролей.</param>
    /// <returns>Список аккаунтов.</returns>
    /// <exception cref="ArgumentException">При пустом списке идентификаторов.</exception>
    public async Task<List<Account>> GetByRole(List<int> roleIds)
    {
        if (roleIds.Count <= 0)
        {
            throw new ArgumentException("Invalid arguments!");
        }

        return await DataContext.Accounts
            .Where(account => roleIds.Contains(account.RoleId))
            .ToListAsync();
    }

    /// <summary>
    ///     Получить список аккаунтов по идентификаторам групп.
    /// </summary>
    /// <param name="groupIds">Идентификаторы групп.</param>
    /// <returns>Список аккаунтов.</returns>
    /// <exception cref="ArgumentException">При пустом списке идентификаторов.</exception>
    public async Task<List<Account>> GetByGroup(List<int> groupIds)
    {
        if (groupIds.Count <= 0)
        {
            throw new ArgumentException("Invalid arguments!");
        }

        return await DataContext.Accounts
            .Where(account => groupIds.Contains(account.GroupId.GetValueOrDefault()))
            .ToListAsync();
    }

    /// <summary>
    ///     Получить список аккаунтов по идентификаторам пользователей.
    /// </summary>
    /// <param name="identityIds">Идентификаторы пользователей.</param>
    /// <returns>Список аккаунтов.</returns>
    /// <exception cref="ArgumentException">При пустом списке идентификаторов.</exception>
    public async Task<List<Account>> GetByIdentityId(List<string> identityIds)
    {
        if (identityIds.Count <= 0)
        {
            throw new ArgumentException("Invalid arguments!");
        }

        return await DataContext.Accounts
            .Where(account => identityIds.Contains(account.IdentityId!))
            .ToListAsync();
    }

    /// <summary>
    ///     Сохранить аккаунты.
    /// </summary>
    /// <param name="accounts">Аккаунты.</param>
    /// <returns>Статус операции.</returns>
    public async Task<bool> Set(List<Account> accounts)
    {
        var identityIds = accounts
            .Where(account => !string.IsNullOrEmpty(account.IdentityId))
            .Select(account => account.IdentityId)
            .ToList();
        var sameUsers = await DataContext.Accounts
            .Where(account => identityIds.Contains(account.IdentityId))
            .ToListAsync();

        if (HasSameIdentities(sameUsers, accounts))
        {
            // Попытка создать аккаунт пользователю,
            // который уже имеет аккаунт.
            return false;
        }

        return await Set(DataContext.Accounts, accounts);
    }

    /// <summary>
    ///     Проверить, что аккаунты для сохранения в базу данных
    ///     не противоречат существующим аккаунтам пользователей.
    ///     Проверяется соответствие идентификаторов аккаунтов
    ///     и идентификаторов пользователей.
    /// </summary>
    /// <param name="existingAccounts">Существующие аккаунты.</param>
    /// <param name="accountsToSave">Аккаунта для сохранения.</param>
    /// <returns>Статус проверки.</returns>
    private static bool HasSameIdentities(List<Account> existingAccounts, List<Account> accountsToSave)
    {
        foreach (var account in existingAccounts)
        {
            var identityId = account.IdentityId;

            if (string.IsNullOrEmpty(identityId))
            {
                continue;
            }

            var sample = accountsToSave.First(x => x.IdentityId == identityId);

            if (sample is null || sample.Id == account.Id)
            {
                continue;
            }

            return true;
        }

        return false;
    }
}