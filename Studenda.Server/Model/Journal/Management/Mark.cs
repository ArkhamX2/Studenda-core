using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Studenda.Server.Data.Configuration;

namespace Studenda.Server.Model.Journal.Management;

/// <summary>
///     Оценка.
/// </summary>
public class Mark : IdentifiableEntity
{
    /*                   __ _                       _   _
     *   ___ ___  _ __  / _(_) __ _ _   _ _ __ __ _| |_(_) ___  _ __
     *  / __/ _ \| '_ \| |_| |/ _` | | | | '__/ _` | __| |/ _ \| '_ \
     * | (_| (_) | | | |  _| | (_| | |_| | | | (_| | |_| | (_) | | | |
     *  \___\___/|_| |_|_| |_|\__, |\__,_|_|  \__,_|\__|_|\___/|_| |_|
     *                        |___/
     * Константы, задающие базовые конфигурации полей
     * и ограничения модели.
     */

    #region Configuration

    public const bool IsTaskIdRequired = true;
    public const bool IsValueRequired = true;

    /// <summary>
    ///     Конфигурация модели.
    /// </summary>
    /// <param name="configuration">Конфигурация базы данных.</param>
    internal class Configuration(ContextConfiguration configuration) : Configuration<Mark>(configuration)
    {
        /// <summary>
        ///     Задать конфигурацию для модели.
        /// </summary>
        /// <param name="builder">Набор интерфейсов настройки модели.</param>
        public override void Configure(EntityTypeBuilder<Mark> builder)
        {
            builder.HasOne(mark => mark.MarkType)
                .WithMany(type => type.Marks)
                .HasForeignKey(mark => mark.MarkTypeId)
                .IsRequired();

            builder.Property(mark => mark.Value)
                .IsRequired();

            base.Configure(builder);
        }
    }

    #endregion

    /*             _   _ _
     *   ___ _ __ | |_(_) |_ _   _
     *  / _ \ '_ \| __| | __| | | |
     * |  __/ | | | |_| | |_| |_| |
     *  \___|_| |_|\__|_|\__|\__, |
     *                       |___/
     * Поля данных, соответствующие таковым в таблице
     * модели в базе данных.
     */

    #region Entity

    /// <summary>
    ///     Идентификатор связанного объекта <see cref="Management.MarkType" />.
    /// </summary>
    public required int MarkTypeId { get; set; }

    /// <summary>
    ///     Значение.
    /// </summary>
    public required int Value { get; set; }

    #endregion

    public MarkType? MarkType { get; set; }
    public List<Task> Tasks { get; set; } = [];
}