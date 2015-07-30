﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace PagedList.Mvc
{
    public static class Extensions
    {

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> html, Expression<Func<TModel, TValue>> expression)
        {
            return DisplayNameForInternal(html, expression);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "This is an extension method")]
        internal static MvcHtmlString DisplayNameForInternal<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> html, Expression<Func<TModel, TValue>> expression)
        {
            return DisplayNameHelper(ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>()),
                                     ExpressionHelper.GetExpressionText(expression));
        }

        internal static MvcHtmlString DisplayNameHelper(ModelMetadata metadata, string htmlFieldName)
        {
            string resolvedDisplayName = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            return new MvcHtmlString(HttpUtility.HtmlEncode(resolvedDisplayName));
        }
    }
}