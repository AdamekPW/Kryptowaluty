using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ASP_.NET_nauka.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(30)]
    public string LastName { get; set; } = null!;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(60)]
    public string Password { get; set; } = null!;

    [Required]
    public int RoleId { get; set; }

    public Role Role { get; set; } = null!;


	public static bool IsEmailCorrect(string Email)
	{
		string Pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
		Regex regex = new Regex(Pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));

		if (!regex.IsMatch(Email)) return false;
		return true;
	}
	public static int PasswordStrongessLevel(string Password)
	{
		string StrongPasswordPattern = @"(?=.*[A-Z])(?=.*[!@#$%^&*(),.?:{}|<>])(?=.*[\d]).{12,}$";
		Regex regex = new Regex(StrongPasswordPattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
		if (regex.IsMatch(Password)) return 3;

		string MediumPasswordPattern = @"(?=.*[A-Z])(?=.*[!@#$%^&*(),.?:{}|<>])(?=.*[\d]).{8,}$";
		regex = new Regex(MediumPasswordPattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
		if (regex.IsMatch(Password)) return 2;

		string WeekPasswordPattern = @"^[\w ]{6,}$";
		regex = new Regex(WeekPasswordPattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
		if (regex.IsMatch(Password)) return 1;
		return 0;
	}

}
