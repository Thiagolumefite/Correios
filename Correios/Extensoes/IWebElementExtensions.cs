using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Correios.Extensoes
{
    public static class IWebElementExtensions
    {
        /// <summary>
        /// retorna o pai do elemento
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public static IWebElement GetParent(this IWebElement webElement)
        {
            return webElement.FindElement(By.XPath(".."));
        }
    }
}
