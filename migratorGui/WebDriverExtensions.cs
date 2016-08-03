﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace migratorGui
{
    static class WebDriverExtensions
    {
        /// <summary>
        /// Find an element, waiting until a timeout is reached if necessary.
        /// </summary>
        /// <param name="context">The search context.</param>
        /// <param name="by">Method to find elements.</param>
        /// <param name="timeout">How many seconds to wait.</param>
        /// <param name="displayed">Require the element to be displayed?</param>
        /// <returns>The found element.</returns>
        public static IWebElement WaitForElement(this ISearchContext context, By by, uint timeout, bool displayed = false)
        {
            var wait = new DefaultWait<ISearchContext>(context);
            wait.Timeout = TimeSpan.FromSeconds(timeout);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(InvalidOperationException));
            return wait.Until(ctx =>
            {
                var elem = ctx.FindElement(by);
                if (displayed && !elem.Displayed)
                    return null;
                return elem;
            });
            //return wait.Until(ctx =>
            //{
            //    IWebElement elem = null;
            //    try
            //    {
            //        elem = ctx.FindElement(by);
            //        if (displayed && !elem.Displayed)
            //        {
            //            elem = null;
            //        }
            //    }
            //    catch (NoSuchElementException nsee)
            //    {

            //    }
            //    catch (InvalidOperationException ioe)
            //    {

            //    }
            //    return elem;
            //});
        }
    }
}
