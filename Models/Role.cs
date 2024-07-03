using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASP_.NET_nauka.Models
{
	public partial class Role
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = null!;

		[Required]
		[Range(0, int.MaxValue)]
		public int Level { get; set; } 
	}
}
