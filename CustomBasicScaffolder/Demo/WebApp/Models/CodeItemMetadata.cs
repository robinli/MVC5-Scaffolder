using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
	 
	public partial class CodeItem
	{
	}

	public partial class CodeItemMetadata
	{
		 

	}




	public class CodeItemChangeViewModel
	{
		public IEnumerable<CodeItem> inserted { get; set; }
		public IEnumerable<CodeItem> deleted { get; set; }
		public IEnumerable<CodeItem> updated { get; set; }
	}

}
