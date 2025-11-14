using System.ComponentModel.DataAnnotations.Schema;

namespace CSDProject.Domain.Entities;

public class User
{
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("password")]
    public string? Password { get; set; }

    [Column("account_status")]
    public string? AccountStatus { get; set; }

    [Column("role")]
    public string? Role { get; set; }

    [Column("device_id")]
    public string? DeviceId { get; set; }

    // Note: is_mobile_device_login removed - not stored in DB, only used for runtime checking
    // Note: IsMobileDeviceLogin will be checked via DeviceId presence instead

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }
}
