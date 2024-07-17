using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
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
    [StringLength(64)]
    public string Password { get; set; } = null!;

    [Required]
    public int RoleId { get; set; }


    //nav properties
    [JsonIgnore]
    public Role Role { get; set; } = null!;

    [JsonIgnore]
    public Wallet Wallet { get; set; } = null!;

    [JsonIgnore]
    public IEnumerable<ActiveOrder> ActiveOrders { get; set; } = null!;

    [JsonIgnore]
    public IEnumerable<CompletedOrder> CompletedOrders { get; set; } = null!;

    public static string CreateSHA256Hash(string Password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(Password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

}
