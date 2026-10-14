using CartService.Domain.Snapshots;
using migApp.Shared.Enums.Characteristics;

namespace CartService.Infrastructure.Data.Seeds;

internal static class CharacteristicSnapshotSeed
{
    public static readonly List<CharacteristicSnapshot> Data =
    [
        // =========================
        // SMARTPHONES
        // =========================
        
        new()
        {
            CharacteristicId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Диагональ экрана",
            CharType = AttributeCharType.Numeric,
            GroupName = "Экран",
            IsUnifying = false,
            CategoryId = Guid.Parse("60000000-0000-0000-0000-000000000001"),
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        
        new()
        {
            CharacteristicId = Guid.Parse("11111111-1111-1111-1111-111111111112"),
            Name = "Разрешение экрана",
            CharType = AttributeCharType.Text,
            GroupName = "Экран",
            IsUnifying = false,
            CategoryId = Guid.Parse("60000000-0000-0000-0000-000000000001"),
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        
        // Единственный объединяющий атрибут
        new()
        {
            CharacteristicId = Guid.Parse("11111111-1111-1111-1111-111111111113"),
            Name = "Модель",
            CharType = AttributeCharType.Text,
            GroupName = "Общие",
            IsUnifying = true,
            CategoryId = Guid.Parse("60000000-0000-0000-0000-000000000001"),
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        
        new()
        {
            CharacteristicId = Guid.Parse("11111111-1111-1111-1111-111111111114"),
            Name = "Цвет",
            CharType = AttributeCharType.Text,
            GroupName = "Общие",
            IsUnifying = false,
            CategoryId = Guid.Parse("60000000-0000-0000-0000-000000000001"),
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        
        new()
        {
            CharacteristicId = Guid.Parse("11111111-1111-1111-1111-111111111115"),
            Name = "Объем памяти",
            CharType = AttributeCharType.Numeric,
            GroupName = "Память",
            IsUnifying = false,
            CategoryId = Guid.Parse("60000000-0000-0000-0000-000000000001"),
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        
        new()
        {
            CharacteristicId = Guid.Parse("11111111-1111-1111-1111-111111111116"),
            Name = "Поддержка 5G",
            CharType = AttributeCharType.Boolean,
            GroupName = "Сеть",
            IsUnifying = false,
            CategoryId = Guid.Parse("60000000-0000-0000-0000-000000000001"),
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        }
    ];
}
