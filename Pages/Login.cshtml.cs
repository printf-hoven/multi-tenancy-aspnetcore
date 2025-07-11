using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MultiTenancy.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MultiTenancy.Pages;

public class LoginModel(DoctorContext _doctorCtx) : PageModel
{
  [TempData]
  public string Message { get; set; } = string.Empty;

  [BindProperty]
  [Display(Name = "Phone:")]
  public string? Phone { get; set; }

  [BindProperty]
  public string? OTP { get; set; }

  public IActionResult OnGet()
  {
    if (true == User.Identity?.IsAuthenticated)
    {
      return LocalRedirect(Url.Page("Index", new { Area = "Members" })!);
    }

    return Page();
  }

  public async Task<IActionResult> OnPost(string? returnUrl)
  {
    if (string.IsNullOrWhiteSpace(Phone))
    {
      return RedirectToPage("Login");
    }

    Doctor? customer = await _doctorCtx.Doctors.FindAsync(Phone.ToUpper());

    if (customer is null)
    {
      customer = new Doctor() { DoctorId = Phone.ToUpper() };

      await _doctorCtx.Doctors.AddAsync(customer);

      await _doctorCtx.SaveChangesAsync();
    }

    if (string.IsNullOrEmpty(OTP))
    {
      // OTP has expired update OTP
      if (customer.OTPValidTill < DateTime.Now)
      {
        // generate new OTP and send
        customer.LastOTP = "1234";

        customer.OTPValidTill = DateTime.Now.AddMinutes(3);

        await _doctorCtx.SaveChangesAsync();

        // send OTP to customer
      }

      return Page();
    }

    if ((customer.OTPValidTill > DateTime.Now) && (string.Equals(customer.LastOTP, OTP)))
    {
      // create auth ticket, etc., and go to members page

      var claims = new List<Claim>
        {
          // store role
          new(ClaimTypes.Role, "member"),
          new(ClaimTypes.Name, customer.DoctorId),
        };

      // create a ClaimsIdentity 
      var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


      // Step 3: signin 
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties() { IsPersistent = true, IssuedUtc = DateTime.UtcNow});

      // home page 
      return LocalRedirect(Url.Page("Index", new { Area = "Members" })!);

    }

    return RedirectToPage("AccessDenied");
  }
}
