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

    [Column("is_mobile_device_login")]
    public bool? IsMobileDeviceLogin { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }
}
