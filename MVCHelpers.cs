using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indasys.Core
{
	public static class MVCHelpers
	{

		//public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes);
		public static string FormLineFor<TModel>(Func<TModel, string> expression, string tag = "p")
		{
			return string.Format("<{0}>{1}</{0}>", tag);
		}
	}
}
