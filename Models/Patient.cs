using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTenancy.Models;

public class Patient
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int PatientId { get; set; }

  // add more fields

}
