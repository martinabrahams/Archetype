using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Archetype.Models;
using Umbraco.Core.Logging;

namespace Archetype.Extensions
{
    /// <summary>
    /// HtmlHelper extenions used for rendering an Archetype.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Renders the archetype partials.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="archetypeModel">The archetype model.</param>
        /// <returns></returns>
        public static IHtmlString RenderArchetypePartials(this HtmlHelper htmlHelper, ArchetypeModel archetypeModel)
        {
            return RenderPartials(htmlHelper, archetypeModel, null, null);
        }

        /// <summary>
        /// Renders the archetype partials.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="archetypeModel">The archetype model.</param>
        /// <param name="partialPath">The partial path.</param>
        /// <returns></returns>
        public static IHtmlString RenderArchetypePartials(this HtmlHelper htmlHelper, ArchetypeModel archetypeModel, string partialPath)
        {
            return RenderPartials(htmlHelper, archetypeModel, partialPath, null);
        }

        /// <summary>
        /// Renders the archetype partials.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="archetypeModel">The archetype model.</param>
        /// <param name="viewDataDictionary">The view data dictionary.</param>
        /// <returns></returns>
        public static IHtmlString RenderArchetypePartials(this HtmlHelper htmlHelper, ArchetypeModel archetypeModel, ViewDataDictionary viewDataDictionary)
        {
            return RenderPartials(htmlHelper, archetypeModel, null, viewDataDictionary);
        }

        /// <summary>
        /// Renders the archetype partials.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="archetypeModel">The archetype model.</param>
        /// <param name="partialPath">The partial path.</param>
        /// <param name="viewDataDictionary">The view data dictionary.</param>
        /// <returns></returns>
        public static IHtmlString RenderArchetypePartials(this HtmlHelper htmlHelper, ArchetypeModel archetypeModel, string partialPath, ViewDataDictionary viewDataDictionary)
        {
            return htmlHelper.RenderPartials(archetypeModel, partialPath, viewDataDictionary);
        }

        /// <summary>
        /// Renders the partials based on the model, partial path and given viewdata dictionary.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="archetypeModel">The archetype model.</param>
        /// <param name="partialPath">The partial path.</param>
        /// <param name="viewDataDictionary">The view data dictionary.</param>
        /// <returns></returns>
        private static IHtmlString RenderPartials(this HtmlHelper htmlHelper, ArchetypeModel archetypeModel, string partialPath, ViewDataDictionary viewDataDictionary)
        {
            var context = HttpContext.Current;

            if (archetypeModel == null || context == null)
            {
                return new HtmlString("");
            }

            var sb = new StringBuilder();

            var pathToPartials = "~/Views/Partials/Archetype/";
            if (!string.IsNullOrEmpty(partialPath))
            {
                pathToPartials = partialPath;
            }

            foreach (var fieldsetModel in archetypeModel)
            {
                var partial = pathToPartials + fieldsetModel.Alias + ".cshtml";

                if (System.IO.File.Exists(context.Server.MapPath(partial)))
                {
                    sb.AppendLine(htmlHelper.Partial(partial, fieldsetModel, viewDataDictionary).ToString());
                }
                else
                {
                    LogHelper.Info<ArchetypeModel>(string.Format("The partial for {0} could not be found.  Please create a partial with that name or rename your alias.", context.Server.MapPath(partial)));
                }
            }

            return new HtmlString(sb.ToString());
        }
    }
}