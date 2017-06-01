using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
	[MetadataType(typeof(CodeItemMetadata))]
	public partial class CodeItem
	{
	}

	public partial class CodeItemMetadata
	{
		[Display(Name = "BaseCode")]
		public BaseCode BaseCode { get; set; }

		[Required(ErrorMessage = "Please enter : Id")]
		[Display(Name = "Id")]
		public int Id { get; set; }

		[Required(ErrorMessage = "Please enter : Code")]
		[Display(Name = "值")]
		[MaxLength(20)]
		public string Code { get; set; }

		[Required(ErrorMessage = "Please enter : Text")]
		[Display(Name = "显示名称")]
		[MaxLength(50)]
		public string Text { get; set; }

		[Display(Name = "代码类别名称")]
		public int BaseCodeId { get; set; }

	}




	public class CodeItemChangeViewModel
	{
		public IEnumerable<CodeItem> inserted { get; set; }
		public IEnumerable<CodeItem> deleted { get; set; }
		public IEnumerable<CodeItem> updated { get; set; }
	}

}
