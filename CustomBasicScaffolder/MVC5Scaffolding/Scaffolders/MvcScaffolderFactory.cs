using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.NuGet;
using Microsoft.AspNet.Scaffolding.EntityFramework.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using Happy.Scaffolding.MVC.UI;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using Happy.Scaffolding.MVC.Scaffolders;

namespace Happy.Scaffolding.MVC
{    

    // This is where everything with the scaffolder is kicked off. The factory
    // returns a MvcScaffolderFactory when a project meets the requirements.
    [Export(typeof(CodeGeneratorFactory))]
    public class MvcScaffolderFactory : CodeGeneratorFactory
    {
        public MvcScaffolderFactory()
            : base(CreateCodeGeneratorInformation())
        {

        }

        public override ICodeGenerator CreateInstance(CodeGenerationContext context)
        {
            return new MvcScaffolder(context, Information);
        }
      
        // We support CSharp WAPs targetting at least .Net Framework 4.5 or above.
        // We DON'T currently support VB
        public override bool IsSupported(CodeGenerationContext codeGenerationContext)
        {
            if (ProjectLanguage.CSharp.Equals(codeGenerationContext.ActiveProject.GetCodeLanguage()) )
            {
                FrameworkName targetFramework = codeGenerationContext.ActiveProject.GetTargetFramework();
                return (targetFramework != null) &&
                        String.Equals(".NetFramework", targetFramework.Identifier, StringComparison.OrdinalIgnoreCase) &&
                        targetFramework.Version >= new Version(4, 5);
            }

            return false;
        }

        private static CodeGeneratorInformation CreateCodeGeneratorInformation()
        {
            return new CodeGeneratorInformation(
                displayName: Resources.MVCScaffolder_Name,
                description: Resources.MVCScaffolder_Description,
                author: "Robin Li",
                version: new Version(0, 1, 5, 27),
                id: typeof(MvcScaffolder).Name,
                icon: ToImageSource(Resources.Application),
                gestures: new[] { "Controller" },
                categories: new[] { Categories.Common, Categories.MvcController, Categories.Other }
            );              
        }

        /// <summary>
        /// Helper method to convert Icon to Imagesource.
        /// </summary>
        /// <param name="icon">Icon</param>
        /// <returns>Imagesource</returns>
        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }


    [Export(typeof(CodeGeneratorFactory))]
    public class MvcScaffolderFactory2 : CodeGeneratorFactory
    {
        public MvcScaffolderFactory2()
            : base(CreateCodeGeneratorInformation())
        {

        }

        public override ICodeGenerator CreateInstance(CodeGenerationContext context)
        {
            return new MvcScaffolderSP(context, Information);
        }

        // We support CSharp WAPs targetting at least .Net Framework 4.5 or above.
        // We DON'T currently support VB
        public override bool IsSupported(CodeGenerationContext codeGenerationContext)
        {
            if (ProjectLanguage.CSharp.Equals(codeGenerationContext.ActiveProject.GetCodeLanguage()))
            {
                FrameworkName targetFramework = codeGenerationContext.ActiveProject.GetTargetFramework();
                return (targetFramework != null) &&
                        String.Equals(".NetFramework", targetFramework.Identifier, StringComparison.OrdinalIgnoreCase) &&
                        targetFramework.Version >= new Version(4, 5);
            }

            return false;
        }

        private static CodeGeneratorInformation CreateCodeGeneratorInformation()
        {
            return new CodeGeneratorInformation(
                displayName: Resources.MVCScaffolder_Name2,
                description: Resources.MVCScaffolder_Description2,
                author: "Robin Li",
                version: new Version(0, 1, 5, 27),
                id: typeof(MvcScaffolder).Name,
                icon: ToImageSource(Resources.Application),
                gestures: new[] { "Controller" },
                categories: new[] { Categories.Common, Categories.MvcController, Categories.Other }
            );
        }

        /// <summary>
        /// Helper method to convert Icon to Imagesource.
        /// </summary>
        /// <param name="icon">Icon</param>
        /// <returns>Imagesource</returns>
        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }
}
