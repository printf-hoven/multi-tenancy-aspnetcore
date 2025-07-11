using System.ComponentModel.DataAnnotations;

namespace MultiTenancy.Models;

public class Doctor
{
  [Key]
  public required string DoctorId { get; set; }

  public string? LastOTP { get; set; }

  public DateTime OTPValidTill { get; set; } = DateTime.MinValue;

  public bool IsPhoneConfirmed {  get; set; } = false;

  // add more fields
}
